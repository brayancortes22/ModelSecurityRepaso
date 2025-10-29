using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class RoleFormPermission : BaseModel
    {
        public int RoleId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
        public  Role Role { get; set; }
        public  Form Form { get; set; }
        public  Permission Permission { get; set; }
    }
}
