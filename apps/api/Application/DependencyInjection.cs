using Microsoft.Extensions.DependencyInjection;
using SearchCount.Api.Services.Tokenazation;
using SearchCount.Api.Services.Aggregation;
using SearchCount.Api.Services;

namespace SearchCount.Api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<QueryTokenizer>();
        services.AddScoped<SearchResultAggregator>();
        services.AddScoped<SearchService>();

        return services;
    }
}