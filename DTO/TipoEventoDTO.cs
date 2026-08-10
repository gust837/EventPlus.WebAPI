using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoEventoDTO
    {
        /// <summary>
        /// Titulo do tipo de usuario
        /// </summary>
        [Required(ErrorMessage = "O titulo é obrigatorio")]
        [StringLength(100, ErrorMessage = "O titulo pode ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
    }
}
