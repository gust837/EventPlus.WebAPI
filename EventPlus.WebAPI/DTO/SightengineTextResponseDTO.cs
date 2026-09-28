using System.Text.Json.Serialization;

namespace EventPlus.WebAPI.DTO.Sightengine
{
    // Representa a resposta da API de Moderação de Texto do Sightengine
    // Doc: https://sightengine.com/docs/text-moderation-rule-based
    public class SightengineTextResponseDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("profanity")]
        public SightengineCategoryDTO? Profanity { get; set; }

        [JsonPropertyName("personal")]
        public SightengineCategoryDTO? Personal { get; set; }

        [JsonPropertyName("link")]
        public SightengineCategoryDTO? Link { get; set; }

        [JsonPropertyName("extremism")]
        public SightengineCategoryDTO? Extremism { get; set; }

        [JsonPropertyName("weapon")]
        public SightengineCategoryDTO? Weapon { get; set; }

        [JsonPropertyName("drug")]
        public SightengineCategoryDTO? Drug { get; set; }

        [JsonPropertyName("medical")]
        public SightengineCategoryDTO? Medical { get; set; }

        [JsonPropertyName("self-harm")]
        public SightengineCategoryDTO? SelfHarm { get; set; }

        [JsonPropertyName("violence")]
        public SightengineCategoryDTO? Violence { get; set; }

        [JsonPropertyName("spam")]
        public SightengineCategoryDTO? Spam { get; set; }

        [JsonPropertyName("content-trade")]
        public SightengineCategoryDTO? ContentTrade { get; set; }

        [JsonPropertyName("money-transaction")]
        public SightengineCategoryDTO? MoneyTransaction { get; set; }

        // Indica se o texto tem ao menos uma ocorrência em qualquer categoria verificada
        [JsonIgnore]
        public bool ContemViolacao =>
            (Profanity?.Matches?.Count ?? 0) > 0 ||
            (Personal?.Matches?.Count ?? 0) > 0 ||
            (Link?.Matches?.Count ?? 0) > 0 ||
            (Extremism?.Matches?.Count ?? 0) > 0 ||
            (Weapon?.Matches?.Count ?? 0) > 0 ||
            (Drug?.Matches?.Count ?? 0) > 0 ||
            (Medical?.Matches?.Count ?? 0) > 0 ||
            (SelfHarm?.Matches?.Count ?? 0) > 0 ||
            (Violence?.Matches?.Count ?? 0) > 0 ||
            (Spam?.Matches?.Count ?? 0) > 0 ||
            (ContentTrade?.Matches?.Count ?? 0) > 0 ||
            (MoneyTransaction?.Matches?.Count ?? 0) > 0;
    }

    public class SightengineCategoryDTO
    {
        [JsonPropertyName("matches")]
        public List<SightengineMatchDTO> Matches { get; set; } = new();
    }

    public class SightengineMatchDTO
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("intensity")]
        public string? Intensity { get; set; }

        [JsonPropertyName("match")]
        public string? Match { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }
    }
}
