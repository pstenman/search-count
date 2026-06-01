using SearchCount.Api.Services.Providers;
using SearchCount.Api.Models;

public class SearchService
{
    private readonly SearchEngineOneClient _engineOne;
    private readonly SearchEngineTwoClient _engineTwo;

    public SearchService(SearchEngineOneClient engineOne, SearchEngineTwoClient engineTwo)
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
            new ProviderCount("engineOne", engineOneTask.Result),
        new ProviderCount("engineTwo", engineTwoTask.Result)
          ]);
    }
}