using API_LCKR.DTOs;
using API_LCKR.Models;
using API_LCKR.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_LCKR.Services
{
    public class AuthService
    {
        private readonly UserRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(UserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public UsuarioDto Login(LoginDto login)
        {
            var findUser = _repository.FindUserEmail(login.Email.Trim());

            if (findUser == null || findUser.Senha != login.Senha)
            {
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
            }

            var expiraEm = DateTime.UtcNow.AddMinutes(_configuration.GetValue("Jwt:ExpiracaoMinutos", 60));

            return new UsuarioDto
            {
                Nome = findUser.Nome,
                Email = findUser.Email,
                Token = GerarToken(findUser, expiraEm),
                ExpiraEm = expiraEm
            };
        }

        private string GerarToken(User user, DateTime expiraEm)
        {
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Nome),
                new Claim("organization_cnpj", user.OrganizationCnpj)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiraEm,
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
