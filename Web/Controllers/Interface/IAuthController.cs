using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ModelSecurityRepaso.Entity.Model;

namespace ModelSecurityRepaso.Web.Controllers.Interface
{
    /// <summary>
    /// Interfaz para el controlador de autenticación.
    /// </summary>
    public interface IAuthController
    {
        Task<IActionResult> Login([FromBody] object loginRequest);
        Task<IActionResult> Refresh([FromBody] object refreshTokenRequest);
        Task<IActionResult> Revoke([FromBody] object refreshTokenRequest);
    }
}
