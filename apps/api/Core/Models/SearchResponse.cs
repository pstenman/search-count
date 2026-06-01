namespace SearchCount.Api.Core.Models;

public sealed record SearchResponse(string Query, IReadOnlyList<ProviderCount> Results, long TotalHits);