using API_LCKR.DTOs;
using API_LCKR.Exceptions;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodoController : ControllerBase
    {
        private readonly PeriodoService _service;

        public PeriodoController(PeriodoService service)
        {
            _service = service;
        }

        [HttpGet("ListarPeriodos")]
        [ProducesResponseType(typeof(List<PeriodoDto>), StatusCodes.Status200OK)]
        public IActionResult ListarPeriodos()
        {
            return Ok(_service.ListarPeriodos());
        }

        [HttpGet("BuscarPeriodo/{mes}")]
        [ProducesResponseType(typeof(PeriodoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        public IActionResult BuscarPeriodo(string mes)
        {
            try
            {
                return Ok(_service.BuscarPeriodo(mes));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
        }

        [HttpPost("FecharPeriodo/{mes}")]
        [ProducesResponseType(typeof(FechamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AvisoFechamentoDto), StatusCodes.Status409Conflict)]
        public IActionResult FecharPeriodo(string mes, [FromQuery] bool confirmar = false)
        {
            try
            {
                return Ok(_service.FecharPeriodo(mes, confirmar));
            }
            catch (ConfirmacaoNecessariaException ex)
            {
                return Conflict(new AvisoFechamentoDto
                {
                    Mensagem = ex.Message,
                    PagamentosPendentes = ex.PagamentosPendentes
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErroDto(ex.Message));
            }
        }

        [HttpPut("ReabrirPeriodo/{mes}")]
        [ProducesResponseType(typeof(PeriodoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult ReabrirPeriodo(string mes)
        {
            try
            {
                return Ok(_service.ReabrirPeriodo(mes));
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
    }
}
