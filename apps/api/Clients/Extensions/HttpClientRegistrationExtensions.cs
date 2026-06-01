using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SearchCount.Api.Clients;
using SearchCount.Api.Configuration;

namespace SearchCount.Api.Clients.Extensions;

public static class HttpClientRegistrationExtensions
{
    public static IServiceCollection AddEngineOneClient(this IServiceCollection services)
    {
        services.AddHttpClient<SearchEngineOneClient>((sp, client) =>
        {
            var cfg = sp.GetRequiredService<IOptions<SearchProvidersConfig>>().Value.EngineOne;

            client.BaseAddress = new Uri(cfg.BaseUrl);
            client.DefaultRequestHeaders.Add("x-api-token", cfg.Token);
        });

        return services;
    }

    public static IServiceCollection AddEngineTwoClient(this IServiceCollection services)
    {
        services.AddHttpClient<SearchEngineTwoClient>((sp, client) =>
        {
            var cfg = sp.GetRequiredService<IOptions<SearchProvidersConfig>>().Value.EngineTwo;

            client.BaseAddress = new Uri(cfg.BaseUrl);
            client.DefaultRequestHeaders.Add("x-api-token", cfg.Token);
        });

        return services;
    }
}