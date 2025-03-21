using Jogo_Cartas.Server.Exception;
using Jogo_Cartas.Server.Models;

namespace Jogo_Cartas.Server.Services.Interfaces
{
    public interface IJogoService
    {
        Task<Baralho> CriarBaralhoAsync();
        Task<List<Jogador>> DistribuirCartasAsync(string deckId, int numeroDeJogadores);
        Task<Baralho> EmbaralharCartasAsync(string deckId);
        Task<(List<(Jogador jogador, Carta carta)> vencedores, string resultado)> CompararCartasAsync(List<Jogador> jogadores);
        Task<Baralho> FinalizarJogoAsync(string deckId);
        object CriarResponseCompararCartas(List<(Jogador jogador, Carta carta)> vencedores, string resultado);
    }
}
