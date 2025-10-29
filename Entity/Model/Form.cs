using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class Form : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public ICollection<RoleFormPermission> RoleFormPermissions { get; set; }
        public ICollection<FormModule> FormModules { get; set; }
    }
}
