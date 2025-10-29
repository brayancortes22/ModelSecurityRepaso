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
    /// Controlador para gestión de usuarios.
    /// Proporciona endpoints CRUD con autenticación y autorización por roles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _business;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor del controlador de usuarios.
        /// </summary>
        /// <param name="business">Servicio de negocio para usuarios</param>
        /// <param name="mapper">Servicio de mapeo de AutoMapper</param>
        public UserController(IUserBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los usuarios del sistema.
        /// </summary>
        /// <returns>Lista de usuarios (datos públicos)</returns>
        /// <response code="200">Lista de usuarios obtenida exitosamente</response>
        /// <response code="401">No autenticado</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            try
            {
                var result = await _business.GetAllAsync();
                var response = _mapper.Map<IEnumerable<UserResponseDto>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un usuario por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            try
            {
                var result = await _business.GetByIdAsync(id);
                if (result == null)
                    return NotFound();
                var response = _mapper.Map<UserResponseDto>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseDto>> Create([FromBody] UserDto dto)
        {
            try
            {
                var entity = _mapper.Map<User>(dto);
                await _business.AddAsync(entity);
                var response = _mapper.Map<UserResponseDto>(entity);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseDto>> Update(int id, [FromBody] UserDto dto)
        {
            try
            {
                var entity = _mapper.Map<User>(dto);
                entity.Id = id;
                await _business.UpdateAsync(entity);
                var result = await _business.GetByIdAsync(id);
                var response = _mapper.Map<UserResponseDto>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un usuario por ID.
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
