namespace SearchCount.Api.Infrastructure.Models;

public sealed record SearchEngineTwoResponse(string Query, long TotalSearchHits);