using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    /// <summary>
    /// Entidad que almacena refresh tokens para renovación de JWT.
    /// Permite al cliente obtener un nuevo access token sin re-autenticarse.
    /// </summary>
    public class RefreshToken : BaseModel
    {
        /// <summary>
        /// ID del usuario propietario del refresh token.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Token alfanumérico aleatorio base64.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de expiración del refresh token.
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Indica si el token ha sido revocado.
        /// </summary>
        public bool IsRevoked { get; set; } = false;

        /// <summary>
        /// Navegación al usuario propietario.
        /// </summary>
        public virtual User? User { get; set; }
    }
}
