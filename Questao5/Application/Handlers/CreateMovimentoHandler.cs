using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public sealed class CreateMovimentoHandler : IRequestHandler<CreateMovimentoCommand, Guid>
    {
        private readonly IContaQueryStore _contas;
        private readonly IMovimentoCommandStore _movimentos;

        public CreateMovimentoHandler(IContaQueryStore contas,
                                      IMovimentoCommandStore movimentos)
        {
            _contas = contas;
            _movimentos = movimentos;
        }

        public async Task<Guid> Handle(CreateMovimentoCommand cmd, CancellationToken ct)
        {
            if (cmd.Valor <= 0) throw new BusinessException(FailType.INVALID_VALUE);
            if (cmd.Tipo is not ('C' or 'D'))
                throw new BusinessException(FailType.INVALID_TYPE);

            var conta = await _contas.ObterContaAtivaPorNumeroAsync(cmd.NumeroConta, ct);
            if (conta is null) throw new BusinessException(FailType.INVALID_ACCOUNT);
            if (!conta.Ativo) throw new BusinessException(FailType.INACTIVE_ACCOUNT);

            // cria entidade do movimento
            var mov = new Movimento
            {
                Id = Guid.NewGuid(),
                IdContaCorrente = conta.Id,
                DataMovimento = DateTime.UtcNow,
                Tipo = cmd.Tipo,
                Valor = cmd.Valor
            };

            // grava com idempotência (se já existir devolve o mesmo Id)
            return await _movimentos.InserirAsync(mov, cmd.RequisicaoId, ct);
        }
    }
}
