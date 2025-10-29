using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ModelSecurityRepaso.Utilities.Exceptions;

namespace ModelSecurityRepaso.Web.Controllers.Implements.Base
{
    /// <summary>
    /// Controlador base genérico para exponer endpoints CRUD estándar.
    /// Incluye manejo de respuestas y errores comunes.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly Business.Interface.Base.IBaseBusiness<T> _business;

        public BaseController(Business.Interface.Base.IBaseBusiness<T> business)
        {
            _business = business;
        }

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(object id)
        {
            try
            {
                var entity = await _business.GetByIdAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Entidad no encontrada." });

                return Ok(entity);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await _business.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Agrega una nueva entidad.
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] T entity)
        {
            try
            {
                await _business.AddAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = entity?.GetType().GetProperty("Id")?.GetValue(entity) }, entity);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(object id, [FromBody] T entity)
        {
            try
            {
                var existing = await _business.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Entidad no encontrada." });

                await _business.UpdateAsync(entity);
                return NoContent();
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza parcialmente una entidad (PATCH).
        /// </summary>
        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(object id, [FromBody] Action<T> patchValues)
        {
            try
            {
                var result = await _business.PatchAsync(id, patchValues);
                if (!result)
                    return NotFound(new { message = "Entidad no encontrada o no se pudo actualizar." });

                return NoContent();
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una entidad físicamente.
        /// </summary>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(object id)
        {
            try
            {
                var entity = await _business.GetByIdAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Entidad no encontrada." });

                await _business.DeleteAsync(entity);
                return NoContent();
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Realiza un borrado lógico de una entidad.
        /// </summary>
        [HttpDelete("logic/{id}")]
        public virtual async Task<IActionResult> DeleteLogical(object id)
        {
            try
            {
                var result = await _business.DeleteLogicalAsync(id);
                if (!result)
                    return NotFound(new { message = "Entidad no encontrada o no se pudo eliminar lógicamente." });

                return NoContent();
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }
    }
}