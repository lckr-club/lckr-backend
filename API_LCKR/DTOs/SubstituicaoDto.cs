using System.ComponentModel.DataAnnotations;

namespace API_LCKR.DTOs
{
    public class SubstituicaoDto
    {
        [Required(ErrorMessage = "O CPF do coach substituto é obrigatório.")]
        public string CpfSubstituto { get; set; } = null!;
    }
}
