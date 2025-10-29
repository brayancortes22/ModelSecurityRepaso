using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con Module.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class ModuleBusiness : BaseBusiness<Module>, IModuleBusiness
    {
        private readonly ModuleData _moduleData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public ModuleBusiness(ModuleData moduleData) : base(moduleData)
        {
            _moduleData = moduleData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
