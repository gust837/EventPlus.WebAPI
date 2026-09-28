using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class ComentarioDTO
    {
        [Required(ErrorMessage = "O comentario não pode ser vazio")]
        [StringLength(100, ErrorMessage = "O comentario pode ter no máximo 100 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        public Guid IdUsuario { get; set; }

        public Guid IdEvento { get; set; }
    }
}
