using SearchCount.Api.Core.Abstractions;
using SearchCount.Api.Core.Models;
using SearchCount.Api.Services.Aggregation;
using SearchCount.Api.Services.Tokenazation;
using Microsoft.Extensions.Logging;

namespace SearchCount.Api.Services;

public class SearchService
{
    private readonly IEnumerable<ISearchEngineClient> _engines;
    private readonly QueryTokenizer _tokenizer;
    private readonly SearchResultAggregator _aggregator;
    private readonly ILogger<SearchService> _logger;

    public SearchService(
        IEnumerable<ISearchEngineClient> engines,
        QueryTokenizer tokenizer,
        SearchResultAggregator aggregator,
        ILogger<SearchService> logger)
    {
        _engines = engines;
        _tokenizer = tokenizer;
        _aggregator = aggregator;
        _logger = logger;
    }

    public async Task<SearchResponse> SearchAsync(string query)
    {
        _logger.LogInformation(
            "Starting search for query '{Query}' using {EngineCount} engines",
            query,
            _engines.Count());

        var terms = _tokenizer.Tokenize(query);

        _logger.LogInformation(
            "Query split into {Count} terms: {Terms}",
            terms.Count(),
            string.Join(", ", terms));

        var tasks =
            from term in terms
            from engine in _engines
            select SearchProviderAsync(engine, term);

        var results = await Task.WhenAll(tasks);

        _logger.LogInformation(
            "Search completed for '{Query}'",
            query);

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
