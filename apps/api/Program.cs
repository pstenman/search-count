using Microsoft.Extensions.Options;
using SearchCount.Api.Configuration;
using SearchCount.Api.Services.Providers;
using SearchCount.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// config binding
builder.Services.Configure<SearchProvidersConfig>(
    builder.Configuration.GetSection("SearchProviders"));

// Http clients
builder.Services.AddHttpClient<SearchEngineOneClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<SearchProvidersConfig>>().Value;

    client.BaseAddress = new Uri(config.EngineOne.BaseUrl);
    client.DefaultRequestHeaders.Add("x-api-token", config.EngineOne.Token);
});

builder.Services.AddHttpClient<SearchEngineTwoClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<SearchProvidersConfig>>().Value;

    client.BaseAddress = new Uri(config.EngineTwo.BaseUrl);
    client.DefaultRequestHeaders.Add("x-api-token", config.EngineTwo.Token);
});

builder.Services.AddScoped<SearchService>();

var app = builder.Build();

// endpoints
app.MapGet("/health", () =>
    Results.Ok(new { status = "ok" }));

app.MapGet("/api/search", async (
    string q,
    SearchService service) =>
{
    if (string.IsNullOrWhiteSpace(q))
    {
        return Results.BadRequest(
            new { error = "Query parameter 'q' is required." });
    }

    var result = await service.SearchAsync(q);

    return Results.Ok(result);
});

app.Run();
