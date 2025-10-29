using ModelSecurityRepaso.Entity.Dto.Base;

namespace ModelSecurityRepaso.Entity.Dto
{
    public class RolFormPermissionDto : BaseDto
    {
        public int RoleId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
    }
}
