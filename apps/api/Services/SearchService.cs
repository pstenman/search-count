using SearchCount.Api.Infrastructure.Clients;
using SearchCount.Api.Core.Models;

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

        var engineOneResult = await engineOneTask;
        var engineTwoResult = await engineTwoTask;

        var results = new List<ProviderCount>
    {
        new("engineOne", engineOneResult),
        new("engineTwo", engineTwoResult)
    };

        var total = engineOneResult + engineTwoResult;

        return new SearchResponse(query, results, total);
    }
}