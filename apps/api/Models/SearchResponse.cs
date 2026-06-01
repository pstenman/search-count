namespace SearchCount.Api.Models;

public sealed record SearchResponse(string Query, IReadOnlyCollection<ProviderCount> Results);