using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ModelSecurityRepaso.Business.Interface.Security;
using ModelSecurityRepaso.Entity.Context;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers
{
    /// <summary>
    /// Controlador para autenticación y gestión de tokens JWT.
    /// Proporciona endpoints para login, refresh de tokens y revocación.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtService;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del controlador de autenticación.
        /// </summary>
        /// <param name="jwtService">Servicio para gestión de tokens JWT</param>
        /// <param name="context">Contexto de base de datos</param>
        public AuthController(IJwtTokenService jwtService, ApplicationDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT de acceso y un refresh token.
        /// </summary>
        /// <param name="request">Credenciales del usuario (email y contraseña)</param>
        /// <returns>Token de acceso JWT, refresh token y tiempo de expiración</returns>
        /// <response code="200">Autenticación exitosa. Retorna tokens.</response>
        /// <response code="400">Solicitud inválida o parámetros faltantes</response>
        /// <response code="401">Credenciales inválidas</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email y contraseña son requeridos");

            // Buscar usuario por email
            var user = _context.User.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Credenciales inválidas");

            // Validar contraseña (en producción, usar bcrypt o similar)
            if (user.Password != request.Password)
                return Unauthorized("Credenciales inválidas");

            // Obtener roles del usuario
            var userRoles = _context.UserRole
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Role, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToList();

            // Generar tokens
            var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, userRoles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Guardar refresh token en la base de datos
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7), // 7 días
                IsRevoked = false
            };

            _context.RefreshToken.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                accessToken,
                refreshToken,
                expiresIn = 3600, // 60 minutos en segundos
                tokenType = "Bearer"
            });
        }

        /// <summary>
        /// Renueva un token JWT usando un refresh token válido.
        /// </summary>
        /// <param name="request">El refresh token previamente generado</param>
        /// <returns>Un nuevo token de acceso JWT</returns>
        /// <response code="200">Nuevo token generado exitosamente</response>
        /// <response code="400">Refresh token no proporcionado</response>
        /// <response code="401">Refresh token inválido, revocado o expirado</response>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Refresh token es requerido");

            // Buscar el refresh token en la base de datos
            var storedToken = _context.RefreshToken
                .FirstOrDefault(rt => rt.Token == request.RefreshToken && !rt.IsRevoked);

            if (storedToken == null)
                return Unauthorized("Refresh token inválido o revocado");

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                storedToken.IsRevoked = true;
                _context.SaveChanges();
                return Unauthorized("Refresh token expirado");
            }

            // Obtener usuario asociado
            var user = _context.User.FirstOrDefault(u => u.Id == storedToken.UserId);
            if (user == null)
                return Unauthorized("Usuario no encontrado");

            // Obtener roles del usuario
            var userRoles = _context.UserRole
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Role, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToList();

            // Generar nuevo access token
            var newAccessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, userRoles);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = request.RefreshToken,
                expiresIn = 3600,
                tokenType = "Bearer"
            });
        }

        /// <summary>
        /// Revoca un refresh token específico, invalidándolo para futuros usos.
        /// </summary>
        /// <param name="request">El refresh token a revocar</param>
        /// <returns>Confirmación de revocación</returns>
        /// <response code="200">Refresh token revocado exitosamente</response>
        /// <response code="400">Refresh token no proporcionado</response>
        /// <response code="404">Refresh token no encontrado</response>
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Refresh token es requerido");

            var storedToken = _context.RefreshToken
                .FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (storedToken == null)
                return NotFound("Refresh token no encontrado");

            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            return Ok("Refresh token revocado exitosamente");
        }
    }

    /// <summary>
    /// Modelo para solicitud de login.
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo para solicitud de refresh token.
    /// </summary>
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
