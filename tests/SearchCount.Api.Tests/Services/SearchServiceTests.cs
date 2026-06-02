using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SearchCount.Api.Core.Abstractions;
using SearchCount.Api.Core.Models;
using SearchCount.Api.Services;
using SearchCount.Api.Services.Aggregation;
using SearchCount.Api.Services.Tokenazation;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace SearchCount.Api.Tests.Services;

public class SearchServiceTests
{
    [Fact]
    public async Task SearchAsync_should_tokenize_and_aggregate_results()
    {
        // Arrange
        var engineOne = new Mock<ISearchEngineClient>();
        var engineTwo = new Mock<ISearchEngineClient>();

        engineOne.Setup(e => e.ProviderName).Returns("engineOne");
        engineTwo.Setup(e => e.ProviderName).Returns("engineTwo");

        engineOne.Setup(e => e.SearchAsync(It.IsAny<string>()))
            .ReturnsAsync(10);

        engineTwo.Setup(e => e.SearchAsync(It.IsAny<string>()))
            .ReturnsAsync(5);

        var engines = new[]
        {
            engineOne.Object,
            engineTwo.Object
        };

        var tokenizer = new QueryTokenizer();
        var aggregator = new SearchResultAggregator();
        var cache = new MemoryCache(new MemoryCacheOptions());

        var service = new SearchService(
            engines,
            tokenizer,
            aggregator,
            NullLogger<SearchService>.Instance,
            cache);

        // Act
        var result = await service.SearchAsync("hello world");

        // Assert

        // hello + world = 2 tokens
        // each engine called twice
        engineOne.Verify(e => e.SearchAsync("hello"), Times.Once);
        engineOne.Verify(e => e.SearchAsync("world"), Times.Once);

        engineTwo.Verify(e => e.SearchAsync("hello"), Times.Once);
        engineTwo.Verify(e => e.SearchAsync("world"), Times.Once);

        result.TotalHits.Should().Be(30);

        result.Results.Should()
            .Contain(x => x.Provider == "engineOne" && x.Count == 20);

        result.Results.Should()
            .Contain(x => x.Provider == "engineTwo" && x.Count == 10);
    }
}