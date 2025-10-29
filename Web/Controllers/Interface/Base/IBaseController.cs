using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ModelSecurityRepaso.Web.Controllers.Interface.Base
{
    /// <summary>
    /// Interfaz para los controladores base con operaciones CRUD estándar.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    public interface IBaseController<T> where T : class
    {
        Task<IActionResult> GetById(object id);
        Task<IActionResult> GetAll();
        Task<IActionResult> Add([FromBody] T entity);
        Task<IActionResult> Update(object id, [FromBody] T entity);
        Task<IActionResult> Delete(object id);
        Task<IActionResult> DeleteLogical(object id);
        Task<IActionResult> Patch(object id, [FromBody] object patchValues);
    }
}