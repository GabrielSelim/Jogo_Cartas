using Jogo_Cartas.Server.Models;

namespace Jogo_Cartas.Server.Services.Interfaces
{
    public interface IClienteAPIService
    {
        Task<Baralho> CriarBaralhoAsync();
        Task<List<Carta>> DistribuirCartasAsync(string deckId, int quantidade);
        Task<Baralho> EmbaralharCartasAsync(string deckId);
        Task<Baralho> FinalizarJogoAsync(string deckId);
        Task<Baralho> ObterBaralhoAsync(string deckId);
    }
}
