using API_LCKR.DTOs;
using API_LCKR.Models;
using API_LCKR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var result = _coachService.CreateCoach(coach);
                return Ok(new
                {
                    result,
                    coach
                });
            }

            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            catch (DbUpdateException)
            {
                return Conflict("Não foi possível cadastrar. Verifique os campos e tente novamente!");
            }
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
            try
            {
                var result = _coachService.UpdateCoach(cpf, coachDto);
                return Ok(new
                {
                    result,
                    coachDto
                });
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            catch (DbUpdateException)
            {
                return Conflict("Não foi possível cadastrar. Verifique os campos e tente novamente!");
            }
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

        [HttpPut("InativarCoach")]
        public IActionResult InativarCoach(string cpf)
        {
            try
            {
                var message = "Coach Inativo!";
                var result = _coachService.InativarCoach(cpf);
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
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
