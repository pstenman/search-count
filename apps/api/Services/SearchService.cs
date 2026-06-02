using SearchCount.Api.Core.Models;
using SearchCount.Api.Infrastructure.Clients;
using SearchCount.Api.Services.Aggregation;
using SearchCount.Api.Services.Tokenazation;

namespace SearchCount.Api.Services;

public class SearchService
{
    private readonly SearchEngineOneClient _engineOne;
    private readonly SearchEngineTwoClient _engineTwo;
    private readonly QueryTokenizer _tokenizer;
    private readonly SearchResultAggregator _aggregator;

    public SearchService(
        SearchEngineOneClient engineOne,
        SearchEngineTwoClient engineTwo,
        QueryTokenizer tokenizer,
        SearchResultAggregator aggregator)
    {
        _engineOne = engineOne;
        _engineTwo = engineTwo;
        _tokenizer = tokenizer;
        _aggregator = aggregator;
    }

    public async Task<SearchResponse> SearchAsync(string query)
    {
        var terms = _tokenizer.Tokenize(query);

        var tasks = terms.Select(async term =>
        {
            var engineOneHits = await _engineOne.SearchAsync(term);
            var engineTwoHits = await _engineTwo.SearchAsync(term);

            return new[]
            {
                new ProviderCount("engineOne", engineOneHits),
                new ProviderCount("engineTwo", engineTwoHits)
            };
        });

        var results = (await Task.WhenAll(tasks))
            .SelectMany(x => x);

        return _aggregator.Aggregate(query, results);
    }
}
