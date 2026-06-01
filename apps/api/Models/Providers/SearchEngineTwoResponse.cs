namespace SearchCount.Api.Models.Providers;

public sealed record SearchEngineTwoResponse(string Query, long TotalSearchHits);