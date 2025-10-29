using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;
using ModelSecurityRepaso.Utilities.Exceptions;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con User.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        private readonly UserData _userData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public UserBusiness(UserData userData) : base(userData)
        {
            _userData = userData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
