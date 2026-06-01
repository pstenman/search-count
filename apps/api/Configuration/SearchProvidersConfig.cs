namespace SearchCount.Api.Configuration;

public sealed class SearchProvidersConfig
{
    public required SearchProviderConfig EngineOne { get; set; }
    public required SearchProviderConfig EngineTwo { get; set; }
}

public sealed class SearchProviderConfig
{
    public required string BaseUrl { get; set; }
    public required string Token { get; set; }
}