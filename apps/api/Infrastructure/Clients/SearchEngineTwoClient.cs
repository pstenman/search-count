using SearchCount.Api.Core.Abstractions;
using SearchCount.Api.Infrastructure.Models;
using System.Net.Http.Json;

namespace SearchCount.Api.Infrastructure.Clients;

public class SearchEngineTwoClient : ISearchEngineClient
{
    private readonly HttpClient _http;

    public string ProviderName => "engineTwo";

    public SearchEngineTwoClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<long> SearchAsync(string query)
    {
        var response = await _http.PostAsJsonAsync(
            "/api/ClassicSongSearchEngine",
            new { query });

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<SearchEngineTwoResponse>();

        return result?.TotalSearchHits ?? 0;
    }
}