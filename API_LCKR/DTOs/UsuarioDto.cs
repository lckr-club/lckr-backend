namespace API_LCKR.DTOs
{
    public class UsuarioDto
    {
        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpiraEm { get; set; }
    }
}
