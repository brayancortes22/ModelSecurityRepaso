using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de usuarios.
    /// </summary>
    public interface IUserController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] User entity);
        Task<IActionResult> Update(int id, [FromBody] User entity);
        Task<IActionResult> Delete(int id);
    }
}
