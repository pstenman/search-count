using SearchCount.Api.Services.Providers.Models;
using System.Net.Http.Json;

namespace SearchCount.Api.Services.Providers;

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

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"SearchEngineTwo failed. Status: {response.StatusCode}");
        }

        var result =
            await response.Content.ReadFromJsonAsync<SearchEngineTwoResponse>();

        return result?.TotalSearchHits ?? 0;
    }
}