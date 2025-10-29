using ModelSecurityRepaso.Entity.Dto.Base;

namespace ModelSecurityRepaso.Entity.Dto
{
    /// <summary>
    /// DTO para transferir datos de RefreshToken en la API.
    /// </summary>
    public class RefreshTokenDto : BaseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
    }
}
