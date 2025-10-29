using ModelSecurityRepaso.Entity.Dto.Base;

namespace ModelSecurityRepaso.Entity.Dto
{
    /// <summary>
    /// DTO para transferir datos de Permission en la API.
    /// </summary>
    public class PermissionDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
