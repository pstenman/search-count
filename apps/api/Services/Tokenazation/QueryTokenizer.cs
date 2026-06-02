using System.Text.RegularExpressions;

namespace SearchCount.Api.Services.Tokenazation;

public class QueryTokenizer
{
    public IReadOnlyList<string> Tokenize(string query)
    {
        return query
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}