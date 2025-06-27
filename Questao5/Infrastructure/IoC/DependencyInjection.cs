using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection svcs)
        {
            svcs.AddSingleton<IMovimentoCommandStore, MovimentoCommandStore>();
            svcs.AddSingleton<IContaQueryStore, ContaQueryStore>();
            return svcs;
        }
    }
}
