namespace Questao5.Application.Queries.Responses
{
    public sealed record GetSaldoResponse(
        int NumeroConta,
        string NomeTitular,
        DateTime DataConsulta,
        decimal Saldo
    );
}
