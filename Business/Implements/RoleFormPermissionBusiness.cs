using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con RoleFormPermission.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class RoleFormPermissionBusiness : BaseBusiness<RoleFormPermission>, IRoleFormPermissionBusiness
    {
        private readonly RoleFormPermissionData _roleFormPermissionData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public RoleFormPermissionBusiness(RoleFormPermissionData roleFormPermissionData) : base(roleFormPermissionData)
        {
            _roleFormPermissionData = roleFormPermissionData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
