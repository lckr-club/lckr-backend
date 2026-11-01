using API_LCKR.DTOs;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status401Unauthorized)]
        public IActionResult Login(LoginDto login)
        {
            try
            {
                return Ok(_authService.Login(login));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErroDto(ex.Message));
            }
        }
    }
}
