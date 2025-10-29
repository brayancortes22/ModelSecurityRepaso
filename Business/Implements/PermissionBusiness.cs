using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.Base;
using ModelSecurityRepaso.Data.Implements;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con Permission.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class PermissionBusiness : BaseBusiness<Permission>
    {
        private readonly PermissionData _permissionData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public PermissionBusiness(PermissionData permissionData) : base(permissionData)
        {
            _permissionData = permissionData;
        }

        // Métodos de validación y lógica pueden agregarse aquí
    }
}
