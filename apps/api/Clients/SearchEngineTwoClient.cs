using SearchCount.Api.Models.Providers;
using System.Net.Http.Json;

namespace SearchCount.Api.Clients;

public class SearchEngineTwoClient
{
    private readonly HttpClient _http;

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