using EventPlus.WebAPI.DTO.Sightengine;
using EventPlus.WebAPI.Interfaces;
using System.Text.Json;

namespace EventPlus.WebAPI.Service
{
    public class ModeracaoTextoService : IModeracaoTextoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        // Categorias verificadas em cada comentário.
        // Ajuste conforme a necessidade (ver categorias em sightengine.com/docs/models-text)
        private const string Categorias = "profanity,personal,link,drug,weapon,spam,extremism,violence,self-harm";

        public ModeracaoTextoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUser = configuration["Sightengine:ApiUser"] ?? string.Empty;
            _apiSecret = configuration["Sightengine:ApiSecret"] ?? string.Empty;
        }

        public async Task<SightengineTextResponseDTO> Analisar(string texto)
        {
            var parametros = new Dictionary<string, string>
            {
                { "text", texto },
                { "lang", "pt" },
                { "mode", "rules,ml" },
                { "categories", Categorias },
                { "api_user", _apiUser },
                { "api_secret", _apiSecret }
            };

            using var conteudo = new FormUrlEncodedContent(parametros);
            using var resposta = await _httpClient.PostAsync("text/check.json", conteudo);

            resposta.EnsureSuccessStatusCode();

            var json = await resposta.Content.ReadAsStringAsync();

            var resultado = JsonSerializer.Deserialize<SightengineTextResponseDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return resultado ?? new SightengineTextResponseDTO();
        }

        public async Task<bool> ContemConteudoImproprio(string texto)
        {
            var resultado = await Analisar(texto);
            return resultado.ContemViolacao;
        }
    }
}
