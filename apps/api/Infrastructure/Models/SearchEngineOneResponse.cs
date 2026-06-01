namespace SearchCount.Api.Infrastructure.Models;

public sealed record SearchEngineOneResponse(string Query, long TotalHits);