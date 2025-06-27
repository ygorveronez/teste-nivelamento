using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public sealed class MovimentoCommandStore : IMovimentoCommandStore
    {
        private readonly DatabaseConfig _cfg;
        public MovimentoCommandStore(DatabaseConfig cfg) => _cfg = cfg;

        public async Task<Guid> InserirAsync(Movimento m, Guid chave, CancellationToken ct)
        {
            await using var conn = new SqliteConnection(_cfg.Name);
            await conn.OpenAsync(ct);                           

            await using var tx = await conn.BeginTransactionAsync(ct);

            var existenteStr = await conn.QueryFirstOrDefaultAsync<string>(
                @"SELECT resultado
                FROM idempotencia
               WHERE chave_idempotencia = @k",
                new { k = chave }, tx);

            if (!string.IsNullOrEmpty(existenteStr))
            {
                await tx.RollbackAsync(ct);
                return Guid.Parse(existenteStr);       
            }

            await conn.ExecuteAsync(
                @"INSERT INTO movimento
                (idmovimento,idcontacorrente,datamovimento,tipomovimento,valor)
              VALUES
                (@Id,@IdContaCorrente,@DataMovimento,@Tipo,@Valor);",
                m, tx);

            await conn.ExecuteAsync(
                @"INSERT INTO idempotencia
                (chave_idempotencia,requisicao,resultado)
              VALUES(@k,@k,@res);",
                new { k = chave, res = m.Id }, tx);

            await tx.CommitAsync(ct);
            return m.Id;
        }
    }
}
