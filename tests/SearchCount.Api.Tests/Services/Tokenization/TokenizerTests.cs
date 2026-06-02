using FluentAssertions;
using SearchCount.Api.Services.Tokenazation;
using Xunit;

namespace SearchCount.Api.Tests.Services.Tokenization;

public class QueryTokenizerTests
{
    [Fact]
    public void Tokenize_should_split_on_space()
    {
        // Arrange
        var tokenizer = new QueryTokenizer();

        // Act
        var result = tokenizer.Tokenize("hello world");

        // Assert
        result.Should().BeEquivalentTo(new[] { "hello", "world" });
    }

    [Fact]
    public void Tokenize_should_trim_and_remove_empty_entries()
    {
        // Arrange
        var tokenizer = new QueryTokenizer();

        // Act
        var result = tokenizer.Tokenize("  hello   world  ");

        // Assert
        result.Should().BeEquivalentTo(new[] { "hello", "world" });
    }

    [Fact]
    public void Tokenize_should_return_empty_list_for_empty_input()
    {
        // Arrange
        var tokenizer = new QueryTokenizer();

        // Act
        var result = tokenizer.Tokenize("");

        // Assert
        result.Should().BeEmpty();
    }
}