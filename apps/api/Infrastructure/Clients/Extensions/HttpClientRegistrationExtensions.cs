using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SearchCount.Api.Infrastructure.Clients;
using SearchCount.Api.Configuration;
using SearchCount.Api.Core.Abstractions;
using Microsoft.Extensions.Http.Resilience;

namespace SearchCount.Api.Infrastructure.Clients.Extensions;

public static class HttpClientRegistrationExtensions
{
    public static IServiceCollection AddEngineOneClient(this IServiceCollection services)
    {
        services.AddHttpClient<SearchEngineOneClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var cfg = sp.GetRequiredService<IOptions<SearchProvidersConfig>>()
                    .Value.EngineOne;

                client.BaseAddress = new Uri(cfg.BaseUrl);
                client.DefaultRequestHeaders.Add("x-api-token", cfg.Token);
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.Delay = TimeSpan.FromSeconds(2);
            });

        services.AddTransient<ISearchEngineClient>(sp =>
            sp.GetRequiredService<SearchEngineOneClient>());

        return services;
    }

    public static IServiceCollection AddEngineTwoClient(this IServiceCollection services)
    {
        services.AddHttpClient<SearchEngineTwoClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var cfg = sp.GetRequiredService<IOptions<SearchProvidersConfig>>()
                    .Value.EngineTwo;

                client.BaseAddress = new Uri(cfg.BaseUrl);
                client.DefaultRequestHeaders.Add("x-api-token", cfg.Token);
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.Delay = TimeSpan.FromSeconds(2);
            });

        services.AddTransient<ISearchEngineClient>(sp =>
            sp.GetRequiredService<SearchEngineTwoClient>());

        return services;
    }
}
