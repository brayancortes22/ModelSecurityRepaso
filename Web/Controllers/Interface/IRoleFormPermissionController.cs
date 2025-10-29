using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de permisos de formularios por roles.
    /// </summary>
    public interface IRoleFormPermissionController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] RoleFormPermission entity);
        Task<IActionResult> Update(int id, [FromBody] RoleFormPermission entity);
        Task<IActionResult> Delete(int id);
    }
}
