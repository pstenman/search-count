using SearchCount.Api.Core.Models;

namespace SearchCount.Api.Services.Aggregation;

public class SearchResultAggregator
{
    public SearchResponse Aggregate(
        string query,
        IEnumerable<ProviderCount> results)
    {
        var grouped = results
            .GroupBy(x => x.Provider)
            .Select(g => new ProviderCount(g.Key, g.Sum(x => x.Count)))
            .ToList();

        var total = grouped.Sum(x => x.Count);

        return new SearchResponse(query, grouped, total);
    }
}