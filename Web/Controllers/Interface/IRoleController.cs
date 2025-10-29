using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de roles.
    /// </summary>
    public interface IRoleController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] Role entity);
        Task<IActionResult> Update(int id, [FromBody] Role entity);
        Task<IActionResult> Delete(int id);
    }
}
