using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ModelSecurityRepaso.Utilities.Security;
using ModelSecurityRepaso.Entity.Context;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers
{
    /// <summary>
    /// Controlador para autenticación y gestión de tokens JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtService;
        private readonly ApplicationDbContext _context;

        public AuthController(IJwtTokenService jwtService, ApplicationDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        /// <summary>
        /// Genera un token JWT basado en credenciales de usuario.
        /// </summary>
        [HttpPost("login")]
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
        [HttpPost("refresh")]
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
        /// Revoca un refresh token específico.
        /// </summary>
        [HttpPost("revoke")]
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
