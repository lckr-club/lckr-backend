using API_LCKR.DTOs;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoValorController : ControllerBase
    {
        private readonly HistoricoValorService _service;

        public HistoricoValorController(HistoricoValorService service)
        {
            _service = service;
        }

        [HttpPost("RegistrarReajuste/{cpf}")]
        [ProducesResponseType(typeof(HistoricoValorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult RegistrarReajuste(string cpf, ReajusteDto reajuste)
        {
            try
            {
                var result = _service.RegistrarReajuste(cpf, reajuste);

                return CreatedAtAction(nameof(ListarHistorico), new { cpf }, result);
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

        [HttpGet("ListarHistorico/{cpf}")]
        [ProducesResponseType(typeof(List<HistoricoValorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        public IActionResult ListarHistorico(string cpf)
        {
            try
            {
                return Ok(_service.ListarHistorico(cpf));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
        }

        [HttpGet("ValorVigente/{cpf}")]
        [ProducesResponseType(typeof(HistoricoValorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        public IActionResult ValorVigente(string cpf, [FromQuery] DateOnly? data)
        {
            try
            {
                return Ok(_service.ValorVigente(cpf, data ?? DateOnly.FromDateTime(DateTime.Today)));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
        }
    }
}
