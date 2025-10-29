using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con FormModule.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class FormModuleBusiness : BaseBusiness<FormModule>, IFormModuleBusiness
    {
        private readonly FormModuleData _formModuleData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public FormModuleBusiness(FormModuleData formModuleData) : base(formModuleData)
        {
            _formModuleData = formModuleData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
