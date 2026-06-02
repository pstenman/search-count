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

// Logging
builder.Host.UseSerilog();

// Cache
builder.Services.AddMemoryCache();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Config
builder.Services.Configure<SearchProvidersConfig>(
    builder.Configuration.GetSection("SearchProviders"));

// Application / Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// API
builder.Services.AddControllers();
var app = builder.Build();

// Middleware
app.UseCors("DevCors");
app.MapControllers();
app.MapGet("/health", () =>
    Results.Ok(new { status = "ok" }));

app.Run();
