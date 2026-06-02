using System.Threading.Tasks;

namespace SearchCount.Api.Core.Abstractions;

public interface ISearchEngineClient
{
    string ProviderName { get; }

    Task<long> SearchAsync(string query);
}