using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de permisos.
    /// </summary>
    public interface IPermissionController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] Permission entity);
        Task<IActionResult> Update(int id, [FromBody] Permission entity);
        Task<IActionResult> Delete(int id);
    }
}
