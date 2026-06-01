using Microsoft.Extensions.Options;
using SearchCount.Api.Configuration;
using SearchCount.Api.Services.Providers;

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

var app = builder.Build();

// endpoints
app.MapGet("/health", () =>
    Results.Ok(new { status = "ok" }));

app.MapGet("/debug/provider1", async (SearchEngineOneClient client) =>
    await client.SearchAsync("react"));

app.MapGet("/debug/provider2", async (SearchEngineTwoClient client) =>
    await client.SearchAsync("react"));

app.Run();
