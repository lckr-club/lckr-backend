using API_LCKR.DTOs;
using API_LCKR.Models;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AulaController : ControllerBase
    {
        private readonly AulaService _service;

        public AulaController(AulaService service)
        {
            _service = service;
        }

        [HttpPost("CriarAula")]
        [ProducesResponseType(typeof(AulaRespostaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult CriarAula(AulaDto aula)
        {
            try
            {
                var result = _service.CriarAula(aula);

                return CreatedAtAction(nameof(BuscarAula), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
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

        [HttpGet("ListarAulas")]
        [ProducesResponseType(typeof(List<AulaRespostaDto>), StatusCodes.Status200OK)]
        public IActionResult ListarAulas([FromQuery] DateOnly? inicio, [FromQuery] DateOnly? fim, [FromQuery] string? cpfCoach)
        {
            return Ok(_service.ListarAulas(inicio, fim, cpfCoach));
        }

        [HttpGet("BuscarAula/{id}")]
        [ProducesResponseType(typeof(AulaRespostaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        public IActionResult BuscarAula(Guid id)
        {
            try
            {
                return Ok(_service.BuscarAula(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
        }

        [HttpPut("AtualizarAula/{id}")]
        [ProducesResponseType(typeof(AulaRespostaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult AtualizarAula(Guid id, AulaDto aula)
        {
            try
            {
                return Ok(_service.AtualizarAula(id, aula));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
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

        [HttpDelete("ExcluirAula/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult ExcluirAula(Guid id)
        {
            try
            {
                _service.ExcluirAula(id);

                return NoContent();
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

        [HttpPut("AlterarStatusAula/{id}")]
        [ProducesResponseType(typeof(AulaRespostaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult AlterarStatusAula(Guid id, [FromQuery] StatusLesson status)
        {
            try
            {
                return Ok(_service.AlterarStatus(id, status));
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

        [HttpPut("SubstituirCoach/{id}")]
        [ProducesResponseType(typeof(AulaRespostaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult SubstituirCoach(Guid id, SubstituicaoDto substituicao)
        {
            try
            {
                return Ok(_service.SubstituirCoach(id, substituicao));
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
