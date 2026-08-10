using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
///Data Transfer Object (DTO) para cadastro e atualizção do perfil do Tipo de Usuario 
/// </summary>
public class TipoUsuarioDTO
{
    /// <summary>
    /// Titulo do tipo de usuario
    /// </summary>
    [Required(ErrorMessage = "O titulo é obrigatorio")]
    [StringLength(100, ErrorMessage = "O titulo pode ter no máximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
}
