using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Language;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/contas")]
    public sealed class ContaController : ControllerBase
    {
        private readonly ISender _sender;
        public ContaController(ISender sender) => _sender = sender;

        /// POST /api/contas/{numero}/movimentos
        [HttpPost("{numero:int}/movimentos")]
        public async Task<IActionResult> Movimentar(int numero, [FromBody] CreateMovimentoBody body)
        {
            try
            {
                var id = await _sender.Send(new CreateMovimentoCommand(
                    body.RequisicaoId, numero, body.Valor, body.Tipo));
                return Ok(new { idMovimento = id });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { tipo = ex.Type, mensagem = ex.Message });
            }
        }

        /// GET /api/contas/{numero}/saldo
        [HttpGet("{numero:int}/saldo")]
        public async Task<IActionResult> Saldo(int numero)
        {
            try
            {
                var resp = await _sender.Send(new GetSaldoQuery(numero));
                return Ok(resp);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { tipo = ex.Type, mensagem = ex.Message });
            }
        }

        public sealed record CreateMovimentoBody(
            Guid RequisicaoId,
            decimal Valor,
            char Tipo);
    }
}
