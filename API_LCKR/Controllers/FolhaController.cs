using API_LCKR.DTOs;
using API_LCKR.Helpers;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_LCKR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolhaController : ControllerBase
    {
        private readonly FolhaService _service;

        public FolhaController(FolhaService service)
        {
            _service = service;
        }

        [HttpGet("CalcularFolha/{mes}")]
        [ProducesResponseType(typeof(FolhaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        public IActionResult CalcularFolha(string mes)
        {
            try
            {
                return Ok(_service.CalcularFolha(mes));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
        }

        [HttpGet("ResumoMes")]
        [ProducesResponseType(typeof(ResumoMesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        public IActionResult ResumoMes([FromQuery] string? mes)
        {
            try
            {
                return Ok(_service.ResumoMes(mes ?? MesReferencia.DaData(DateOnly.FromDateTime(DateTime.Today))));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
        }

        [HttpGet("ExportarPdf/{mes}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/pdf")]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        public IActionResult ExportarPdf(string mes)
        {
            try
            {
                return File(_service.ExportarPdf(mes), "application/pdf", $"folha-{mes}.pdf");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
        }

        [HttpGet("ExportarExcel/{mes}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        [ProducesResponseType(typeof(ErroDto), StatusCodes.Status400BadRequest)]
        public IActionResult ExportarExcel(string mes)
        {
            try
            {
                return File(_service.ExportarExcel(mes), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"folha-{mes}.xlsx");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErroDto(ex.Message));
            }
        }
    }
}
