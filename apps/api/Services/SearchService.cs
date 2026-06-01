using SearchCount.Api.Clients;
using SearchCount.Api.Models;

namespace SearchCount.Api.Services;

public class SearchService
{
    private readonly SearchEngineOneClient _engineOne;
    private readonly SearchEngineTwoClient _engineTwo;

    public SearchService(
        SearchEngineOneClient engineOne,
        SearchEngineTwoClient engineTwo)
    {
        _engineOne = engineOne;
        _engineTwo = engineTwo;
    }

    public async Task<SearchResponse> SearchAsync(string query)
    {
        var engineOneTask = _engineOne.SearchAsync(query);
        var engineTwoTask = _engineTwo.SearchAsync(query);

        await Task.WhenAll(engineOneTask, engineTwoTask);

        return new SearchResponse(
            query,
            [
                new ProviderCount("engineOne", await engineOneTask),
                new ProviderCount("engineTwo", await engineTwoTask)
            ]);
    }
}