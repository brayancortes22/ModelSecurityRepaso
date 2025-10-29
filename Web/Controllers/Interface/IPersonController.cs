using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de personas.
    /// </summary>
    public interface IPersonController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] Person entity);
        Task<IActionResult> Update(int id, [FromBody] Person entity);
        Task<IActionResult> Delete(int id);
    }
}
