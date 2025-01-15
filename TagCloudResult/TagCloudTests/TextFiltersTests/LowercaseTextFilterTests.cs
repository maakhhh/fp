using FluentAssertions;
using TagCloud.TextFilters;

namespace TagCloudTests.TextFiltersTests;

[TestFixture]
public class LowercaseTextFilterTests
{
    private LowercaseTextFilter filter = new();

    [Test]
    public void ReturnEmptyEnumerable_WhenTextIsEmpty()
    {
        var result = filter.Apply([]);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [TestCase(new string[1] { "a" }, new string[1] { "a" }, TestName = "Do nothing")]
    [TestCase(new string[1] { "A" }, new string[1] { "a" }, TestName = "One word")]
    [TestCase(new string[3] { "a", "A", "B" }, new string[3] { "a", "a", "b" }, TestName = "Several words")]
    public void FilterApplyCorrect(string[] words, string[] expectedResult)
    {
        var result = filter.Apply(words);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
    }
}
