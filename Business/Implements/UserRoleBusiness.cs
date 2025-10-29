using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con UserRole.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class UserRoleBusiness : BaseBusiness<UserRole>, IUserRoleBusiness
    {
        private readonly UserRoleData _userRoleData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public UserRoleBusiness(UserRoleData userRoleData) : base(userRoleData)
        {
            _userRoleData = userRoleData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
