using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de roles de usuarios.
    /// </summary>
    public interface IUserRoleController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Create([FromBody] UserRole entity);
        Task<IActionResult> Update(int id, [FromBody] UserRole entity);
        Task<IActionResult> Delete(int id);
    }
}
