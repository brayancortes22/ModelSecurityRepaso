using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class Module : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<FormModule> FormModules { get; set; }
    }
}
