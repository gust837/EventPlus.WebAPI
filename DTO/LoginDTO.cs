using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O email é obrigatório!")]
        [EmailAddress(ErrorMessage = "Informe um email válido!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória!")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 a 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}
