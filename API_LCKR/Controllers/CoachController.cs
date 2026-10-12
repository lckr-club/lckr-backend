using API_LCKR.DTOs;
using API_LCKR.Models;
using API_LCKR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

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
        public IActionResult AddCoach(CoachDto coach)
        {
            var result = _coachService.CreateCoach(coach);

            if (result == "Coach já está cadastrado!")
            {
                return BadRequest(result);
            }

            return Ok(new
            {
                result,
                coach
            });
        }

        [HttpGet("ListarCoaches")]
        public IActionResult ListCoaches()
        {
            var result = _coachService.ListCoaches();

            return Ok(result);
        }

        [HttpPut("AtualizarCoach/{cpf}")]
        public IActionResult UpdateCoach(string cpf, CoachDto coachDto)
        {
            var result = _coachService.UpdateCoach(cpf, coachDto);
            
            if (result == "Cpf não foi encotrado")
            {
                return BadRequest(result);
            }
            return Ok(new
            {
                result,
                coachDto
            });
        }

        [HttpGet("BuscarCoach/{cpf}")]
        public IActionResult GetCoach(string cpf)
        {
            try
            {
                var result = _coachService.GetCoach(cpf);
                return Ok(result);

            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeletarCoach")]
        public IActionResult DeteteCoach(string cpf)
        {
            try
            {
                var message = "Coach removido!";
                var result = _coachService.DeleteCoach(cpf);
                return Ok(new
                {
                    message,
                    result
                });

            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
        }
    }
}
