using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class ComentarioDTO
    {
        [Required(ErrorMessage = "O comentario não pode ser vazio")]
        [StringLength(100, ErrorMessage = "O comentario pode ter no máximo 100 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data do comentario é obrigatória")]
        public DateTime DataComentario { get; set; }

        public Guid IdUsuario { get; set; }

        public Guid IdEvento { get; set; }
    }
}
