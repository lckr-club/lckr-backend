using API_LCKR.DTOs;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly CoachService _coachService;

        public CoachController(CoachService coachService)
        {
            _coachService = coachService;
        }

        [HttpPost("CriarCoach")]
        [ProducesResponseType(typeof(CoachDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult AddCoach(CoachDto coach)
        {
            try
            {
                var result = _coachService.CreateCoach(coach);

                return CreatedAtAction(nameof(GetCoach), new { cpf = result.Cpf }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErroDto(ex.Message));
            }
            catch (DbUpdateException)
            {
                return Conflict(new ErroDto("Não foi possível cadastrar. Verifique os campos e tente novamente!"));
            }
        }

        [HttpGet("ListarCoaches")]
        [ProducesResponseType(typeof(List<CoachDto>), StatusCodes.Status200OK)]
        public IActionResult ListCoaches()
        {
            return Ok(_coachService.ListCoaches());
        }

        [HttpGet("BuscarCoach/{cpf}")]
        [ProducesResponseType(typeof(CoachDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        public IActionResult GetCoach(string cpf)
        {
            try
            {
                return Ok(_coachService.GetCoach(cpf));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
        }

        [HttpPut("AtualizarCoach/{cpf}")]
        [ProducesResponseType(typeof(CoachDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult UpdateCoach(string cpf, CoachDto coachDto)
        {
            try
            {
                return Ok(_coachService.UpdateCoach(cpf, coachDto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErroDto(ex.Message));
            }
            catch (DbUpdateException)
            {
                return Conflict(new ErroDto("Não foi possível atualizar. Verifique os campos e tente novamente!"));
            }
        }

        [HttpPut("InativarCoach")]
        [ProducesResponseType(typeof(CoachDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult InativarCoach(string cpf)
        {
            try
            {
                return Ok(_coachService.InativarCoach(cpf));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErroDto(ex.Message));
            }
        }

        [HttpPut("ReativarCoach")]
        [ProducesResponseType(typeof(CoachDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult ReativarCoach(string cpf)
        {
            try
            {
                return Ok(_coachService.ReativarCoach(cpf));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErroDto(ex.Message));
            }
        }
    }
}
