using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para cadastro e atualização do perfil do Usuário
/// </summary>
public class UsuarioDTO
{
    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um email válido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário (obrigatória no cadastro; opcional na edição para manter a senha atual)
    /// </summary>
    [StringLength(60, ErrorMessage = "A senha pode ter no máximo 60 caracteres.")]
    public string? Senha { get; set; }

    public Guid? IdTipoUsuario { get; set; }
}
