namespace ModelSecurityRepaso.Entity.Dto.Response
{
    /// <summary>
    /// DTO de respuesta para RefreshToken - Expone solo datos públicos sin información sensible.
    /// </summary>
    public class RefreshTokenResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
