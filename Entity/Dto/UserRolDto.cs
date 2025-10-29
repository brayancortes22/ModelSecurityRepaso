using ModelSecurityRepaso.Entity.Dto.Base;

namespace ModelSecurityRepaso.Entity.Dto
{
    public class UserRolDto : BaseDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
