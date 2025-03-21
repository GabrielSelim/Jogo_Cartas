using Jogo_Cartas.Server.Services.Interfaces;
using Jogo_Cartas.Server.Exception;
using Jogo_Cartas.Server.Models;

namespace Jogo_Cartas.Server.Services
{
    public class JogoServico : IJogoService
    {
        private readonly IClienteAPIService _clienteApi;
        private const int MaxJogadores = 10;

        public JogoServico(IClienteAPIService clienteApi)
        {
            _clienteApi = clienteApi;
        }

        public async Task<Baralho> CriarBaralhoAsync()
        {
            try
            {
                return await _clienteApi.CriarBaralhoAsync();
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                throw new ApiException("A API está fora do ar. Tente novamente mais tarde.");
            }
        }

        public async Task<List<Jogador>> DistribuirCartasAsync(string deckId, int numeroDeJogadores)
        {
            try
            {
                var baralho = await _clienteApi.ObterBaralhoAsync(deckId);
                ValidarBaralho(baralho, deckId);
                ValidarNumeroDeJogadores(numeroDeJogadores);

                var jogadores = new List<Jogador>();
                for (int i = 0; i < numeroDeJogadores; i++)
                {
                    var cartas = await _clienteApi.DistribuirCartasAsync(deckId, 5);
                    jogadores.Add(new Jogador { Nome = $"Jogador {i + 1}", Cartas = cartas });
                }

                return jogadores;
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                throw new ApiException("A API está fora do ar. Tente novamente mais tarde.");
            }
        }

        private void ValidarBaralho(Baralho baralho, string deckId)
        {
            if (baralho == null)
            {
                throw new ApiException($"Baralho com ID {deckId} não encontrado.");
            }

            if (!baralho.Embaralhado)
            {
                throw new ApiException("O baralho precisa ser embaralhado antes de distribuir as cartas.");
            }

            if (baralho.CartasRestantes != 52)
            {
                throw new ApiException("As cartas só podem ser distribuídas novamente se o baralho for embaralhado.");
            }
        }

        private void ValidarNumeroDeJogadores(int numeroDeJogadores)
        {
            if (numeroDeJogadores < 2)
            {
                throw new ApiException("O número de jogadores deve ser pelo menos 2.");
            }

            if (numeroDeJogadores > MaxJogadores)
            {
                throw new ApiException($"O número máximo de jogadores é {MaxJogadores}.");
            }
        }

        public async Task<Baralho> ObterBaralhoAsync(string deckId)
        {
            try
            {
                return await _clienteApi.ObterBaralhoAsync(deckId);
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                throw new ApiException("A API está fora do ar. Tente novamente mais tarde.");
            }
        }

        public async Task<Baralho> EmbaralharCartasAsync(string deckId)
        {
            try
            {
                return await _clienteApi.EmbaralharCartasAsync(deckId);
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                throw new ApiException("A API está fora do ar. Tente novamente mais tarde.");
            }
        }

        public async Task<(List<(Jogador jogador, Carta carta)> vencedores, string resultado)> CompararCartasAsync(List<Jogador> jogadores)
        {
            if (jogadores == null || !jogadores.Any())
            {
                throw new ApiException("A lista de jogadores não pode ser nula ou vazia.");
            }

            var valores = ObterValoresDasCartas();
            var vencedores = new List<(Jogador jogador, Carta carta)>();
            int maiorValor = 0;

            foreach (var jogador in jogadores)
            {
                ValidarCartasDoJogador(jogador);

                var melhorCarta = jogador.Cartas
                    .Where(carta => ValidarCarta(carta, valores))
                    .OrderByDescending(carta => valores[carta.Valor])
                    .FirstOrDefault();

                if (melhorCarta != null)
                {
                    int valorMelhorCarta = valores[melhorCarta.Valor];

                    if (valorMelhorCarta > maiorValor)
                    {
                        maiorValor = valorMelhorCarta;
                        vencedores.Clear();
                        vencedores.Add((jogador, melhorCarta));
                    }
                    else if (valorMelhorCarta == maiorValor)
                    {
                        vencedores.Add((jogador, melhorCarta));
                    }
                }
            }

            if (vencedores.Count == 0)
            {
                throw new ApiException("Nenhum vencedor encontrado.");
            }

            string resultado = vencedores.Count > 1 ? "Empate" : "Vitória";

            return (vencedores, resultado);
        }

        private void ValidarCartasDoJogador(Jogador jogador)
        {
            if (jogador.Cartas.Count < 5)
            {
                throw new ApiException($"O jogador {jogador.Nome} tem menos de 5 cartas.");
            }
        }

        private Dictionary<string, int> ObterValoresDasCartas()
        {
            return new Dictionary<string, int>
            {
                { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }, { "6", 6 }, { "7", 7 }, { "8", 8 }, { "9", 9 }, { "10", 10 },
                { "JACK", 11 }, { "QUEEN", 12 }, { "KING", 13 }, { "ACE", 14 }
            };
        }

        private bool ValidarCarta(Carta carta, Dictionary<string, int> valores)
        {
            if (carta == null || string.IsNullOrEmpty(carta.Valor) || string.IsNullOrEmpty(carta.Naipe) || string.IsNullOrEmpty(carta.Codigo) || string.IsNullOrEmpty(carta.Imagem))
            {
                throw new ApiException("Carta inválida: todos os campos devem ser preenchidos.");
            }

            if (!valores.ContainsKey(carta.Valor))
            {
                throw new ApiException($"Valor de carta inválido: {carta.Valor}");
            }

            return true;
        }

        public async Task<Baralho> FinalizarJogoAsync(string deckId)
        {
            try
            {
                var response = await _clienteApi.FinalizarJogoAsync(deckId);
                return response;
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                throw new ApiException("A API está fora do ar. Tente novamente mais tarde.");
            }
        }

        public object CriarResponseCompararCartas(List<(Jogador jogador, Carta carta)> vencedores, string resultado)
        {
            var response = vencedores.Select(v => new { v.jogador.Nome, Carta = v.carta }).ToList();
            return new { vencedores = response, resultado };
        }
    }
}
