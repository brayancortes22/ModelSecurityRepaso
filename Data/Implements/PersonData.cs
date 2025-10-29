using ModelSecurityRepaso.Entity.Context;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Data.Implements.Base;
using ModelSecurityRepaso.Data.Interface;

namespace ModelSecurityRepaso.Data.Implements
{
    /// <summary>
    /// Repositorio específico para operaciones con la entidad Person.
    /// Extiende BaseData con métodos personalizados si es necesario.
    /// </summary>
    public class PersonData : BaseData<Person>, IPersonData
    {
        /// <summary>
        /// Constructor que recibe el contexto de BD por inyección.
        /// </summary>
        public PersonData(ApplicationDbContext context) : base(context) { }

        // Métodos personalizados pueden agregarse aquí
    }
}
