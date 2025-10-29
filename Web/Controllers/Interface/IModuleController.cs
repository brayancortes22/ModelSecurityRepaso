using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de módulos.
    /// </summary>
    public interface IModuleController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] Module entity);
        Task<IActionResult> Update(int id, [FromBody] Module entity);
        Task<IActionResult> Delete(int id);
    }
}
