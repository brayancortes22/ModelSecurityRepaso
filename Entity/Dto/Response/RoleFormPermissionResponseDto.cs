namespace ModelSecurityRepaso.Entity.Dto.Response
{
    /// <summary>
    /// DTO de respuesta para RoleFormPermission - Expone solo datos públicos.
    /// </summary>
    public class RoleFormPermissionResponseDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
