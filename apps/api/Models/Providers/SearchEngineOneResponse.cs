namespace SearchCount.Api.Models.Providers;

public sealed record SearchEngineOneResponse(string Query, long TotalHits);