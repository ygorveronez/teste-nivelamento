using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public interface IContaQueryStore
    {
        Task<ContaCorrente?> ObterContaAtivaPorNumeroAsync(int numero, CancellationToken ct);
        Task<decimal> ObterSaldoAsync(Guid idConta, CancellationToken ct);
    }
}
