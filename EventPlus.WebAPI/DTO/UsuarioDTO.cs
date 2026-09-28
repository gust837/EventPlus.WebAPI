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
    [StringLength(100, ErrorMessage = "O Nome pode ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuario
    /// </summary>
    [Required(ErrorMessage = "Campo obrigatorio")]
    [EmailAddress(ErrorMessage = "O email é obrigatório.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuario
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 60 caracteres")]
    public string Senha { get; set; } = string.Empty;

    public Guid? IdTipoUsuario { get; set;  }
}