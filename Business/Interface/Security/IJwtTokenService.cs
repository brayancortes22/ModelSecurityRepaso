using System.Security.Claims;

namespace ModelSecurityRepaso.Business.Interface.Security
{
    /// <summary>
    /// Servicio para generación y validación de tokens JWT.
    /// Maneja tokens de acceso y refresh tokens.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Genera un nuevo JWT token de acceso.
        /// </summary>
        string GenerateAccessToken(int userId, string email, List<string> roles);

        /// <summary>
        /// Genera un nuevo refresh token aleatorio.
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Valida un JWT token y retorna los claims.
        /// </summary>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Obtiene los claims del token.
        /// </summary>
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
