using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers
{
    /// <summary>
    /// Controlador base para gestionar permisos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionBusiness _business;

        public PermissionController(IPermissionBusiness business)
        {
            _business = business;
        }

        /// <summary>
        /// Obtiene todos los permisos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _business.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _business.GetByIdAsync(id);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo permiso.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Permission entity)
        {
            try
            {
                var result = await _business.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un permiso existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Permission entity)
        {
            try
            {
                entity.Id = id;
                await _business.UpdateAsync(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un permiso por ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _business.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();

                await _business.DeleteAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
