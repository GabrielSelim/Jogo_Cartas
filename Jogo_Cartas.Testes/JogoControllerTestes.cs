using Jogo_Cartas.Server.Controllers;
using Jogo_Cartas.Server.Exception;
using Jogo_Cartas.Server.Models;
using Jogo_Cartas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jogo_Cartas.Testes
{
    public class JogoControllerTestes
    {
        private readonly Mock<IJogoService> _jogoServiceMock;
        private readonly JogoController _controller;

        public JogoControllerTestes()
        {
            _jogoServiceMock = new Mock<IJogoService>();
            _controller = new JogoController(_jogoServiceMock.Object);
        }

        [Fact(DisplayName = "CriarBaralho deve retornar OkResult com o Baralho criado")]
        public async Task CriarBaralho_ReturnsOkResult_WithBaralho()
        {
            var baralho = new Baralho { Id = "deck1", Embaralhado = true, CartasRestantes = 52 };
            _jogoServiceMock.Setup(service => service.CriarBaralhoAsync()).ReturnsAsync(baralho);

            var result = await _controller.CriarBaralho();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Baralho>(okResult.Value);
            Assert.Equal(baralho.Id, returnValue.Id);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar BadRequest se a API estiver fora do ar")]
        public async Task DistribuirCartas_ReturnsBadRequest_IfApiIsDown()
        {
            var deckId = "deck1";
            var numeroDeJogadores = 4;

            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync(deckId, numeroDeJogadores))
                .ThrowsAsync(new ApiException("A API está fora do ar. Tente novamente mais tarde."));

            var result = await _controller.DistribuirCartas(deckId, numeroDeJogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("A API está fora do ar. Tente novamente mais tarde.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar OkResult com a lista de jogadores")]
        public async Task DistribuirCartas_ReturnsOkResult_WithJogadores()
        {
            var jogadores = new List<Jogador>
            {
                new Jogador { Nome = "Jogador 1", Cartas = new List<Carta>
                {
                    new Carta { Valor = "ACE", Naipe = "HEARTS", Codigo = "AH", Imagem = "https://deckofcardsapi.com/static/img/AH.png" }
                }},
                new Jogador { Nome = "Jogador 2", Cartas = new List<Carta>
                {
                    new Carta { Valor = "KING", Naipe = "SPADES", Codigo = "KS", Imagem = "https://deckofcardsapi.com/static/img/KS.png" }
                }}
            };
            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync("deck1", 2)).ReturnsAsync(jogadores);

            var result = await _controller.DistribuirCartas("deck1", 2);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Jogador>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar BadRequest se o número de jogadores for menor que 2")]
        public async Task DistribuirCartas_ReturnsBadRequest_IfNumeroDeJogadoresIsLessThanOne()
        {
            var deckId = "deck1";
            var numeroDeJogadores = 0;

            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync(deckId, numeroDeJogadores))
                .ThrowsAsync(new ApiException("O número de jogadores deve ser pelo menos 1."));

            var result = await _controller.DistribuirCartas(deckId, numeroDeJogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("O número de jogadores deve ser pelo menos 1.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar BadRequest se o número de jogadores for maior que o máximo permitido")]
        public async Task DistribuirCartas_ReturnsBadRequest_IfNumeroDeJogadoresExceedsMax()
        {
            var deckId = "deck1";
            var numeroDeJogadores = 11;

            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync(deckId, numeroDeJogadores))
                .ThrowsAsync(new ApiException("O número máximo de jogadores é 10."));

            var result = await _controller.DistribuirCartas(deckId, numeroDeJogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("O número máximo de jogadores é 10.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar BadRequest se o baralho não for encontrado")]
        public async Task DistribuirCartas_ReturnsBadRequest_IfDeckNotFound()
        {
            var deckId = "invalidDeckId";
            var numeroDeJogadores = 4;

            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync(deckId, numeroDeJogadores))
                .ThrowsAsync(new ApiException("Baralho com ID inválido não encontrado."));

            var result = await _controller.DistribuirCartas(deckId, numeroDeJogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Baralho com ID inválido não encontrado.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar BadRequest se o baralho não tiver 52 cartas")]
        public async Task DistribuirCartas_ReturnsBadRequest_IfDeckNotFull()
        {
            var deckId = "testDeckId";
            var numeroDeJogadores = 4;

            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync(deckId, numeroDeJogadores))
                .ThrowsAsync(new ApiException("As cartas só podem ser distribuídas novamente se o baralho for embaralhado."));

            var result = await _controller.DistribuirCartas(deckId, numeroDeJogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("As cartas só podem ser distribuídas novamente se o baralho for embaralhado.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "CompararCartas deve retornar OkResult com os vencedores")]
        public async Task CompararCartas_ReturnsOkResult_WithVencedores()
        {
            var jogadores = new List<Jogador>
            {
                new Jogador { Nome = "Jogador 1", Cartas = new List<Carta> { new Carta { Valor = "ACE", Naipe = "HEARTS", Codigo = "AH", Imagem = "https://deckofcardsapi.com/static/img/AH.png" } } },
                new Jogador { Nome = "Jogador 2", Cartas = new List<Carta> { new Carta { Valor = "KING", Naipe = "SPADES", Codigo = "KS", Imagem = "https://deckofcardsapi.com/static/img/KS.png" } } }
            };
            var vencedores = new List<(Jogador jogador, Carta carta)>
            {
                (jogadores[0], jogadores[0].Cartas[0])
            };
            _jogoServiceMock.Setup(service => service.CompararCartasAsync(jogadores)).ReturnsAsync((vencedores, "Vitória"));
            _jogoServiceMock.Setup(service => service.CriarResponseCompararCartas(vencedores, "Vitória"))
                .Returns(new { vencedores = vencedores.Select(v => new { v.jogador.Nome, Carta = v.carta }).ToList(), resultado = "Vitória" });

            var result = await _controller.CompararCartas(jogadores);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value as dynamic;

            Assert.NotNull(returnValue);

            var vencedoresList = returnValue.vencedores as IEnumerable<dynamic>;
            var resultado = returnValue.resultado as string;

            Assert.NotNull(vencedoresList);
            Assert.Single(vencedoresList);

            var vencedor = vencedoresList.First();
            var nome = vencedor.Nome as string;
            var carta = vencedor.Carta as dynamic;

            var valor = carta.Valor as string;

            Assert.Equal("Vitória", resultado);
            Assert.Equal("Jogador 1", nome);
            Assert.Equal("ACE", valor);
        }

        [Fact(DisplayName = "CompararCartas deve retornar OkResult com empate")]
        public async Task CompararCartas_ReturnsOkResult_WithEmpate()
        {
            var jogadores = new List<Jogador>
            {
                new Jogador { Nome = "Jogador 1", Cartas = new List<Carta> { new Carta { Valor = "ACE", Naipe = "HEARTS", Codigo = "AH", Imagem = "https://deckofcardsapi.com/static/img/AH.png" } } },
                new Jogador { Nome = "Jogador 2", Cartas = new List<Carta> { new Carta { Valor = "ACE", Naipe = "SPADES", Codigo = "AS", Imagem = "https://deckofcardsapi.com/static/img/AS.png" } } }
            };
            var vencedores = new List<(Jogador jogador, Carta carta)>
            {
                (jogadores[0], jogadores[0].Cartas[0]),
                (jogadores[1], jogadores[1].Cartas[0])
            };
            _jogoServiceMock.Setup(service => service.CompararCartasAsync(jogadores)).ReturnsAsync((vencedores, "Empate"));
            _jogoServiceMock.Setup(service => service.CriarResponseCompararCartas(vencedores, "Empate"))
                .Returns(new { vencedores = vencedores.Select(v => new { v.jogador.Nome, Carta = v.carta }).ToList(), resultado = "Empate" });

            var result = await _controller.CompararCartas(jogadores);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value as dynamic;

            Assert.NotNull(returnValue);

            var vencedoresList = returnValue.vencedores as IEnumerable<dynamic>;
            var resultado = returnValue.resultado as string;

            Assert.NotNull(vencedoresList);
            Assert.Equal(2, vencedoresList.Count());

            var vencedor1 = vencedoresList.First();
            var vencedor2 = vencedoresList.Last();

            var nome1 = vencedor1.Nome as string;
            var carta1 = vencedor1.Carta as dynamic;

            var nome2 = vencedor2.Nome as string;
            var carta2 = vencedor2.Carta as dynamic;

            var valor1 = carta1.Valor as string;
            var valor2 = carta2.Valor as string;

            Assert.Equal("Empate", resultado);
            Assert.Equal("Jogador 1", nome1);
            Assert.Equal("ACE", valor1);
            Assert.Equal("Jogador 2", nome2);
            Assert.Equal("ACE", valor2);
        }

        [Fact(DisplayName = "FinalizarJogo deve retornar OkResult com o Baralho finalizado")]
        public async Task FinalizarJogo_ReturnsOkResult_WithBaralho()
        {
            var baralho = new Baralho { Id = "deck1", Embaralhado = false, CartasRestantes = 0 };
            _jogoServiceMock.Setup(service => service.FinalizarJogoAsync("deck1")).ReturnsAsync(baralho);

            var result = await _controller.FinalizarJogo("deck1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Baralho>(okResult.Value);
            Assert.Equal(0, returnValue.CartasRestantes);
        }

        [Fact(DisplayName = "DistribuirCartas deve retornar BadRequest se o baralho não estiver embaralhado")]
        public async Task DistribuirCartas_ReturnsBadRequest_IfDeckNotShuffled()
        {
            var deckId = "deck1";
            var numeroDeJogadores = 4;

            _jogoServiceMock.Setup(service => service.DistribuirCartasAsync(deckId, numeroDeJogadores))
                .ThrowsAsync(new ApiException("O baralho precisa ser embaralhado antes de distribuir as cartas."));

            var result = await _controller.DistribuirCartas(deckId, numeroDeJogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("O baralho precisa ser embaralhado antes de distribuir as cartas.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "CompararCartas deve retornar BadRequest se a lista de jogadores for nula ou vazia")]
        public async Task CompararCartas_ReturnsBadRequest_IfJogadoresListIsNullOrEmpty()
        {
            List<Jogador> jogadores = null;

            _jogoServiceMock.Setup(service => service.CompararCartasAsync(jogadores))
                .ThrowsAsync(new ApiException("A lista de jogadores não pode ser nula ou vazia."));

            var result = await _controller.CompararCartas(jogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value as ApiErrorResponse;
            Assert.NotNull(returnValue);
            Assert.Equal("A lista de jogadores não pode ser nula ou vazia.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "CompararCartas deve retornar BadRequest se algum jogador tiver menos de 5 cartas")]
        public async Task CompararCartas_ReturnsBadRequest_IfAnyJogadorHasLessThanFiveCards()
        {
            var jogadores = new List<Jogador>
            {
                new Jogador { Nome = "Jogador 1", Cartas = new List<Carta> { new Carta { Valor = "ACE", Naipe = "HEARTS", Codigo = "AH", Imagem = "https://deckofcardsapi.com/static/img/AH.png" } } }
            };

            _jogoServiceMock.Setup(service => service.CompararCartasAsync(jogadores))
                .ThrowsAsync(new ApiException("O jogador Jogador 1 tem menos de 5 cartas."));

            var result = await _controller.CompararCartas(jogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value as ApiErrorResponse;
            Assert.NotNull(returnValue);
            Assert.Equal("O jogador Jogador 1 tem menos de 5 cartas.", returnValue.Mensagem);
        }

        [Fact(DisplayName = "CompararCartas deve retornar BadRequest se algum campo da carta for inválido")]
        public async Task CompararCartas_ReturnsBadRequest_IfAnyCardFieldIsInvalid()
        {
            var jogadores = new List<Jogador>
            {
                new Jogador { Nome = "Jogador 1", Cartas = new List<Carta> { new Carta { Valor = "", Naipe = "HEARTS", Codigo = "AH", Imagem = "https://deckofcardsapi.com/static/img/AH.png" } } }
            };

            _jogoServiceMock.Setup(service => service.CompararCartasAsync(jogadores))
                .ThrowsAsync(new ApiException("Carta inválida: todos os campos devem ser preenchidos."));

            var result = await _controller.CompararCartas(jogadores);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value as ApiErrorResponse;
            Assert.NotNull(returnValue);
            Assert.Equal("Carta inválida: todos os campos devem ser preenchidos.", returnValue.Mensagem);
        }
    }
}