using MediatR;

namespace Questao5.Application.Commands.Requests
{
    public sealed record CreateMovimentoCommand(
        Guid RequisicaoId,
        int NumeroConta,
        decimal Valor,
        char Tipo  // 'C' | 'D'
    ) : IRequest<Guid>;


}
