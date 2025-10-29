using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con Form.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class FormBusiness : BaseBusiness<Form>, IFormBusiness
    {
        private readonly FormData _formData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public FormBusiness(FormData formData) : base(formData)
        {
            _formData = formData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
