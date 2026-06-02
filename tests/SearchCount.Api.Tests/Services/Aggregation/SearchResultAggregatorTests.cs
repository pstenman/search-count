using FluentAssertions;
using SearchCount.Api.Core.Models;
using SearchCount.Api.Services.Aggregation;
using Xunit;

namespace SearchCount.Api.Tests.Services.Aggregation;

public class SearchResultAggregatorTests
{
    [Fact]
    public void Aggregate_should_sum_hits_per_provider_and_total()
    {
        // Arrange
        var aggregator = new SearchResultAggregator();

        var input = new[]
        {
            new ProviderCount("engineOne", 10),
            new ProviderCount("engineOne", 5),
            new ProviderCount("engineTwo", 7)
        };

        // Act
        var result = aggregator.Aggregate("query", input);

        // Assert
        result.Results.Should().ContainSingle(x => x.Provider == "engineOne" && x.Count == 15);
        result.Results.Should().ContainSingle(x => x.Provider == "engineTwo" && x.Count == 7);
        result.TotalHits.Should().Be(22);
    }

    [Fact]
    public void Aggregate_should_return_zero_for_empty_input()
    {
        // Arrange
        var aggregator = new SearchResultAggregator();

        var input = Array.Empty<ProviderCount>();

        // Act
        var result = aggregator.Aggregate("query", input);

        // Assert
        result.TotalHits.Should().Be(0);
        result.Results.Should().BeEmpty();
    }

    [Fact]
    public void Aggregate_should_group_multiple_providers_correctly()
    {
        // Arrange
        var aggregator = new SearchResultAggregator();

        var input = new[]
        {
            new ProviderCount("engineOne", 1),
            new ProviderCount("engineTwo", 2),
            new ProviderCount("engineOne", 3),
            new ProviderCount("engineTwo", 4)
        };

        // Act
        var result = aggregator.Aggregate("query", input);

        // Assert
        result.Results.Should().Contain(x => x.Provider == "engineOne" && x.Count == 4);
        result.Results.Should().Contain(x => x.Provider == "engineTwo" && x.Count == 6);
        result.TotalHits.Should().Be(10);
    }
}