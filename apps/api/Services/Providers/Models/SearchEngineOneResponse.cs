namespace SearchCount.Api.Services.Providers.Models;

public sealed record SearchEngineOneResponse(string Query, long TotalHits);