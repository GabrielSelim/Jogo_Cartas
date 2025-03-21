namespace Jogo_Cartas.Server.Models
{
    public class Jogador
    {
        public required string Nome { get; set; }
        public List<Carta> Cartas { get; set; } = new List<Carta>();
    }
}
