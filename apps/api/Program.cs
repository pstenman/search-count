using SearchCount.Api.Configuration;
using SearchCount.Api.Application;
using SearchCount.Api.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "SearchCount.Api")
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Cache
builder.Services.AddMemoryCache();

// Logging
builder.Host.UseSerilog();

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
