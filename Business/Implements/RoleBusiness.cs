using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con Role.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class RoleBusiness : BaseBusiness<Role>
    {
        private readonly RoleData _roleData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public RoleBusiness(RoleData roleData) : base(roleData)
        {
            _roleData = roleData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
