using API_LCKR.DTOs;
using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class AuthService
    {
        private readonly UserRepository _repository;

        public AuthService(UserRepository repository)
        {
            _repository = repository;
        }

        public UsuarioDto Login(LoginDto login)
        {
            var findUser = _repository.FindUserEmail(login.Email.Trim());

            if (findUser == null || findUser.Senha != login.Senha)
            {
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
            }

            return new UsuarioDto
            {
                Nome = findUser.Nome,
                Email = findUser.Email
            };
        }
    }
}
