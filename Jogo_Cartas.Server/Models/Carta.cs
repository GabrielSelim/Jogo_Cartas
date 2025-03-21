using System.Text.Json.Serialization;

namespace Jogo_Cartas.Server.Models
{
    public class Carta
    {
        [JsonPropertyName("code")]
        public required string Codigo { get; set; }

        [JsonPropertyName("image")]
        public required string Imagem { get; set; }

        [JsonPropertyName("value")]
        public required string Valor { get; set; }

        [JsonPropertyName("suit")]
        public required string Naipe { get; set; }
    }
}
