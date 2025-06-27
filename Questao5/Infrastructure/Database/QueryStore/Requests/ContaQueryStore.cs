using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public sealed class ContaQueryStore : IContaQueryStore
    {
        private readonly DatabaseConfig _cfg;
        public ContaQueryStore(DatabaseConfig cfg) => _cfg = cfg;

        public async Task<ContaCorrente?> ObterContaAtivaPorNumeroAsync(int numero, CancellationToken ct)
        {
            await using var conn = new SqliteConnection(_cfg.Name);
            await conn.OpenAsync(ct);                           // <<< NOVO

            var obj = await conn.QueryFirstOrDefaultAsync(
                @"SELECT idcontacorrente AS Id,
                     numero          AS Numero,
                     nome            AS Nome,
                     ativo           AS Ativo
              FROM contacorrente
              WHERE numero = @n",
                new { n = numero });

            return obj is null ? null : new ContaCorrente
            {
                Id = Guid.Parse((string)obj.Id),
                Numero = (int)obj.Numero,
                Nome = (string)obj.Nome,
                Ativo = ((long)obj.Ativo) == 1
            };
        }

        public async Task<decimal> ObterSaldoAsync(Guid idConta, CancellationToken ct)
        {
            await using var conn = new SqliteConnection(_cfg.Name);
            await conn.OpenAsync(ct);                           // <<< NOVO

            return await conn.QueryFirstAsync<decimal>(
                @"SELECT COALESCE(SUM(
                     CASE tipomovimento WHEN 'C' THEN valor ELSE -valor END),0)
              FROM movimento
              WHERE idcontacorrente = @id",
                new { id = idConta });
        }
    }
}
