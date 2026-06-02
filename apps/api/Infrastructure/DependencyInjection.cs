using Microsoft.Extensions.DependencyInjection;
using SearchCount.Api.Infrastructure.Clients.Extensions;

namespace SearchCount.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddEngineOneClient();
        services.AddEngineTwoClient();

        return services;
    }
}