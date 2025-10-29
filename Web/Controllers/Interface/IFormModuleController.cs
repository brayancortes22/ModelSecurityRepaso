using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de módulos de formularios.
    /// </summary>
    public interface IFormModuleController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] FormModule entity);
        Task<IActionResult> Update(int id, [FromBody] FormModule entity);
        Task<IActionResult> Delete(int id);
    }
}
