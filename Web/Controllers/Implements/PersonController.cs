using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto;
using ModelSecurityRepaso.Entity.Dto.Response;
using AutoMapper;

namespace ModelSecurityRepaso.Web.Controllers
{
    /// <summary>
    /// Controlador para gestionar personas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonBusiness _business;
        private readonly IMapper _mapper;

        public PersonController(IPersonBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las personas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonResponseDto>>> GetAll()
        {
            try
            {
                var result = await _business.GetAllAsync();
                var response = _mapper.Map<IEnumerable<PersonResponseDto>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una persona por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonResponseDto>> GetById(int id)
        {
            try
            {
                var result = await _business.GetByIdAsync(id);
                if (result == null)
                    return NotFound();
                var response = _mapper.Map<PersonResponseDto>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva persona.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PersonResponseDto>> Create([FromBody] PersonDto dto)
        {
            try
            {
                var entity = _mapper.Map<Person>(dto);
                await _business.AddAsync(entity);
                var response = _mapper.Map<PersonResponseDto>(entity);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una persona existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PersonResponseDto>> Update(int id, [FromBody] PersonDto dto)
        {
            try
            {
                var entity = _mapper.Map<Person>(dto);
                entity.Id = id;
                await _business.UpdateAsync(entity);
                var result = await _business.GetByIdAsync(id);
                var response = _mapper.Map<PersonResponseDto>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una persona por ID.
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
