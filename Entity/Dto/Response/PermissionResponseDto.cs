namespace ModelSecurityRepaso.Entity.Dto.Response
{
    /// <summary>
    /// DTO de respuesta para Permission - Expone solo datos públicos.
    /// </summary>
    public class PermissionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
