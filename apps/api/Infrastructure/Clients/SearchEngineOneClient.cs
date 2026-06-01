using SearchCount.Api.Infrastructure.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace SearchCount.Api.Infrastructure.Clients;

public class SearchEngineOneClient
{
    private readonly HttpClient _http;

    public SearchEngineOneClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<long> SearchAsync(string query)
    {
        var url = QueryHelpers.AddQueryString(
            "/api/AltavistaSearchEngine",
            "query",
            query);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<SearchEngineOneResponse>();

        return result?.TotalHits ?? 0;
    }
}
