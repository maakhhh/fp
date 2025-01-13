using FluentAssertions;
using TagCloud.TextFilters;

namespace TagCloudTests.TextFiltersTests;

[TestFixture]
public class BoringTextFilterTests
{
    private BoringTextFilter filter = new();

    [Test]
    public void BeEmpty_WhenWordsIsEmpty()
    {
        var result = filter.Apply([]);

        result.Should().BeEmpty();
    }

    [TestCase(new string[1] { "hello" }, new string[1] { "hello" }, TestName = "Not skip simple words")]
    [TestCase(new string[1] { "a" }, new string[0], TestName = "Skip articles")]
    [TestCase(new string[3] { "at", "in", "on" }, new string[0], TestName = "Skip prepositions")]
    [TestCase(new string[3] { "he", "she", "it" }, new string[0], TestName = "Skip pronouns")]
    [TestCase(new string[2] { "at", "since" }, new string[0], TestName = "Skip conjunctions")]
    public void ApplyCorrectly(string[] words, string[] expectedResult)
    {
        var result = filter.Apply(words);
        result.Should().BeEquivalentTo(expectedResult);
    }
}
