using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class InstituicaoDTO
    {
        /// <summary>
        /// Nome fantasia da instituição
        /// </summary>
        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter apenas 100 caracteres.")]
        public string NomeFantasia { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, ErrorMessage = "O CNPJ pode ter apenas 14 caracteres.")]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(100, ErrorMessage = "O endereço pode ter apenas 100 caracteres.")]
        public string Endereco { get; set; } = string.Empty;
    }
}
