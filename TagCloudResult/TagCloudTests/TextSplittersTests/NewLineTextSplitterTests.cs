using FluentAssertions;
using TagCloud.TextSplitters;

namespace TagCloudTests.TextSplittersTests;

[TestFixture]
public class NewLineTextSplitterTests
{
    private readonly NewLineTextSplitter splitter = new();

    [Test]
    public void ResultWithoutError_WhenEmptyText()
    {
        var words = splitter.Split(string.Empty);
        words.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void ErrorResult_WhenTextIsNull()
    {
        var words = splitter.Split(null);
        words.IsSuccess.Should().BeFalse();
    }

    [TestCase(new string[] { "a" }, new string[] { "a" }, TestName = "One word")]
    [TestCase(new string[] { "a", "b" }, new string[] { "a", "b" }, TestName = "Several words")]
    [TestCase(new string[] { "a", "", "b" }, new string[] { "a", "b" }, TestName = "Skip empty words")]
    public void SplitTextCorrect(string[] words, string[] expectedResult)
    {
        var text = string.Join(Environment.NewLine, words);
        var actualResult = splitter.Split(text);
        
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value!.ToArray().Should().BeEquivalentTo(expectedResult);
    }
}
