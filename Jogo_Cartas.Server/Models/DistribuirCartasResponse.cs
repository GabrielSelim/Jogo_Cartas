using System.Text.Json.Serialization;

namespace Jogo_Cartas.Server.Models
{
    public class DistribuirCartasResponse
    {
        [JsonPropertyName("cards")]
        public required List<Carta> Cartas { get; set; }
    }
}
