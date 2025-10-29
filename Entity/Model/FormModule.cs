using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class FormModule : BaseModel
    {
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public int FormId { get; set; }

        public Module Module { get; set; }
        public Form Form { get; set; }
    }
}
