using SearchCount.Api.Configuration;
using SearchCount.Api.Application;
using SearchCount.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Services.Configure<SearchProvidersConfig>(
    builder.Configuration.GetSection("SearchProviders"));

// Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// MVC
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/health", () =>
    Results.Ok(new { status = "ok" }));

app.Run();
