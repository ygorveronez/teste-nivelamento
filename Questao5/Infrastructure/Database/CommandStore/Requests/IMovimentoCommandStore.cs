using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public interface IMovimentoCommandStore
    {
        Task<Guid> InserirAsync(Movimento mov, Guid chaveIdempotencia, CancellationToken ct);
    }
}
