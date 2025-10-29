using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de formularios.
    /// </summary>
    public interface IFormController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] Form entity);
        Task<IActionResult> Update(int id, [FromBody] Form entity);
        Task<IActionResult> Delete(int id);
    }
}
