using SearchCount.Api.Core.Abstractions;
using SearchCount.Api.Core.Models;
using SearchCount.Api.Services.Aggregation;
using SearchCount.Api.Services.Tokenazation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace SearchCount.Api.Services;

public class SearchService
{
    private readonly IEnumerable<ISearchEngineClient> _engines;
    private readonly QueryTokenizer _tokenizer;
    private readonly SearchResultAggregator _aggregator;
    private readonly ILogger<SearchService> _logger;
    private readonly IMemoryCache _cache;
    public SearchService(
        IEnumerable<ISearchEngineClient> engines,
        QueryTokenizer tokenizer,
        SearchResultAggregator aggregator,
        ILogger<SearchService> logger,
        IMemoryCache cache)
    {
        _engines = engines;
        _tokenizer = tokenizer;
        _aggregator = aggregator;
        _logger = logger;
        _cache = cache;
    }

    public async Task<SearchResponse> SearchAsync(string query)
    {
        var terms = _tokenizer.Tokenize(query).ToArray();

        var normalizedKey = string.Join('|', terms.OrderBy(t => t));
        var cacheKey = $"search:{normalizedKey}";

        if (_cache.TryGetValue(cacheKey, out SearchResponse? cached))
        {
            _logger.LogInformation(
                "Cache hit for query '{Query}'",
                query);

            return cached!;
        }

        _logger.LogInformation(
            "Cache miss for query '{Query}'",
            query);

        _logger.LogInformation(
            "Starting search for query '{Query}' using {EngineCount} engines",
            query,
            _engines.Count());

        _logger.LogInformation(
            "Query split into {Count} terms: {Terms}",
            terms.Length,
            string.Join(", ", terms));

        var tasks =
            from term in terms
            from engine in _engines
            select SearchProviderAsync(engine, term);

        var results = await Task.WhenAll(tasks);

        var response = _aggregator.Aggregate(query, results);

        _cache.Set(
            cacheKey,
            response,
            TimeSpan.FromMinutes(5));

        _logger.LogInformation(
            "Search completed for '{Query}' with total hits {TotalHits}",
            query,
            response.TotalHits);

        return response;
    }

    private async Task<ProviderCount> SearchProviderAsync(
        ISearchEngineClient engine,
        string term)
    {
        try
        {
            var hits = await engine.SearchAsync(term);

            return new ProviderCount(
                engine.ProviderName,
                hits);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Engine {Engine} failed for term '{Term}'",
                engine.ProviderName,
                term);

            return new ProviderCount(
                engine.ProviderName,
                0);
        }
    }
}
