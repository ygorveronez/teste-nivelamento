using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public sealed record GetSaldoQuery(int NumeroConta) : IRequest<GetSaldoResponse>;
}
