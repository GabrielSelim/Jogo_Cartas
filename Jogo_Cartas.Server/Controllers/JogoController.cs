using Jogo_Cartas.Server.Exception;
using Jogo_Cartas.Server.Models;
using Jogo_Cartas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jogo_Cartas.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _jogoServico;

        public JogoController(IJogoService jogoServico)
        {
            _jogoServico = jogoServico;
        }

        [HttpPost("criar-baralho")]
        public async Task<ActionResult<Baralho>> CriarBaralho()
        {
            try
            {
                var baralho = await _jogoServico.CriarBaralhoAsync();
                return Ok(baralho);
            }
            catch (ApiException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("distribuir-cartas")]
        public async Task<ActionResult<List<Jogador>>> DistribuirCartas([FromQuery] string deckId, [FromQuery] int numeroDeJogadores)
        {
            try
            {
                var jogadores = await _jogoServico.DistribuirCartasAsync(deckId, numeroDeJogadores);
                return Ok(jogadores);
            }
            catch (ApiException ex)
            {
                return BadRequest(new ApiErrorResponse { Mensagem = ex.Message });
            }
        }

        [HttpPost("embaralhar-cartas")]
        public async Task<ActionResult<Baralho>> EmbaralharCartas([FromQuery] string deckId)
        {
            try
            {
                var baralho = await _jogoServico.EmbaralharCartasAsync(deckId);
                return Ok(baralho);
            }
            catch (ApiException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("comparar-cartas")]
        public async Task<ActionResult> CompararCartas([FromBody] List<Jogador> jogadores)
        {
            try
            {
                var (vencedores, resultado) = await _jogoServico.CompararCartasAsync(jogadores);
                var response = _jogoServico.CriarResponseCompararCartas(vencedores, resultado);
                return Ok(response);
            }
            catch (ApiException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("finalizar-jogo")]
        public async Task<ActionResult<Baralho>> FinalizarJogo([FromQuery] string deckId)
        {
            try
            {
                var baralho = await _jogoServico.FinalizarJogoAsync(deckId);
                return Ok(baralho);
            }
            catch (ApiException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
