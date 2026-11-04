using API_LCKR.DTOs;
using API_LCKR.Models;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly PagamentoService _service;

        public PagamentoController(PagamentoService service)
        {
            _service = service;
        }

        [HttpGet("ListarPagamentos")]
        [ProducesResponseType(typeof(List<PagamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        public IActionResult ListarPagamentos([FromQuery] string? mes, [FromQuery] string? cpfCoach, [FromQuery] StatusPagamento? status)
        {
            try
            {
                return Ok(_service.ListarPagamentos(mes, cpfCoach, status));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
        }

        [HttpGet("BuscarPagamento/{id}")]
        [ProducesResponseType(typeof(PagamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        public IActionResult BuscarPagamento(Guid id)
        {
            try
            {
                return Ok(_service.BuscarPagamento(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErroDto(ex.Message));
            }
        }

        [HttpPut("MarcarPago/{id}")]
        [ProducesResponseType(typeof(PagamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status409Conflict)]
        public IActionResult MarcarPago(Guid id, [FromQuery] DateOnly? pagoEm)
        {
            try
            {
                return Ok(_service.MarcarPago(id, pagoEm));
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
