namespace SearchCount.Api.Models;

public sealed record SearchResponse(string Query, IReadOnlyList<ProviderCount> Results, long TotalHits);