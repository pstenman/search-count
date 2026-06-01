using SearchCount.Api.Configuration;
using SearchCount.Api.Services;
using SearchCount.Api.Clients.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Services.Configure<SearchProvidersConfig>(
    builder.Configuration.GetSection("SearchProviders"));

// Http clients
builder.Services.AddEngineOneClient();
builder.Services.AddEngineTwoClient();

// Services
builder.Services.AddScoped<SearchService>();

// MVC
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/health", () =>
    Results.Ok(new { status = "ok" }));

app.Run();
