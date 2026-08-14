using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
///Data Transfer Object (DTO) para cadastro e atualizção do perfil do Usuario 
/// </summary>
public class UsuarioDTO
{
    /// <summary>
    /// Nome do usuario
    /// </summary>
    [Required(ErrorMessage = "O titulo é obrigatorio")]
    [StringLength(100, ErrorMessage = "O titulo pode ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuario
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatorio")]
    [StringLength(100, ErrorMessage = "O email pode ter no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuario
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(64, ErrorMessage = "A senha pode ter no máximo 60 caracteres")]
    public string Senha { get; set; } = string.Empty;
}