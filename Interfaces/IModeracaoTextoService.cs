using EventPlus.WebAPI.DTO.Sightengine;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IModeracaoTextoService
    {
        // Envia o texto para o Sightengine e retorna a resposta bruta da análise
        Task<SightengineTextResponseDTO> Analisar(string texto);

        // Retorna true se o texto violar alguma categoria monitorada
        Task<bool> ContemConteudoImproprio(string texto);
    }
}
