using SearchCount.Api.Core.Abstractions;
using SearchCount.Api.Core.Models;
using SearchCount.Api.Services.Aggregation;
using SearchCount.Api.Services.Tokenazation;

namespace SearchCount.Api.Services;

public class SearchService
{
    private readonly IEnumerable<ISearchEngineClient> _engines;
    private readonly QueryTokenizer _tokenizer;
    private readonly SearchResultAggregator _aggregator;

    public SearchService(
        IEnumerable<ISearchEngineClient> engines,
        QueryTokenizer tokenizer,
        SearchResultAggregator aggregator)
    {
        _engines = engines;
        _tokenizer = tokenizer;
        _aggregator = aggregator;
    }

    public async Task<SearchResponse> SearchAsync(string query)
    {
        var terms = _tokenizer.Tokenize(query);

        var tasks =
            from term in terms
            from engine in _engines
            select SearchProviderAsync(engine, term);

        var results = await Task.WhenAll(tasks);

        return _aggregator.Aggregate(query, results);
    }

    private static async Task<ProviderCount> SearchProviderAsync(
        ISearchEngineClient engine,
        string term)
    {
        var hits = await engine.SearchAsync(term);

        return new ProviderCount(
            engine.ProviderName,
            hits);
    }
}
