using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class Permission : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public  ICollection<RoleFormPermission> RoleFormPermissions { get; set; }
    }
}
