using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con Person.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class PersonBusiness : BaseBusiness<Person>, IPersonBusiness
    {
        private readonly PersonData _personData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public PersonBusiness(PersonData personData) : base(personData)
        {
            _personData = personData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
