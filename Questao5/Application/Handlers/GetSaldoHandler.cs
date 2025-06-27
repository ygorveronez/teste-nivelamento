using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public sealed class GetSaldoHandler : IRequestHandler<GetSaldoQuery, GetSaldoResponse>
    {
        private readonly IContaQueryStore _contas;
        public GetSaldoHandler(IContaQueryStore contas) => _contas = contas;

        public async Task<GetSaldoResponse> Handle(GetSaldoQuery q, CancellationToken ct)
        {
            var conta = await _contas.ObterContaAtivaPorNumeroAsync(q.NumeroConta, ct);
            if (conta is null) throw new BusinessException(FailType.INVALID_ACCOUNT);
            if (!conta.Ativo) throw new BusinessException(FailType.INACTIVE_ACCOUNT);

            var saldo = await _contas.ObterSaldoAsync(conta.Id, ct);

            return new GetSaldoResponse(
                conta.Numero,
                conta.Nome,
                DateTime.UtcNow,
                saldo);
        }
    }
}
