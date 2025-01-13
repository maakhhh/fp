using FluentAssertions;
using TagCloud.TextSplitters;

namespace TagCloudTests.TextSplittersTests;

[TestFixture]
public class NewLineTextSplitterTests
{
    private readonly NewLineTextSplitter splitter = new();

    [Test]
    public void NotThrow_WhenEmptyText()
    {
        var action = () => splitter.Split(string.Empty);
        action.Should().NotThrow();
    }

    [Test]
    public void Throw_WhenTextIsNull()
    {
        var action = () => splitter.Split(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [TestCase(new string[] { "a" }, new string[] { "a" }, TestName = "One word")]
    [TestCase(new string[] { "a", "b" }, new string[] { "a", "b" }, TestName = "Several words")]
    [TestCase(new string[] { "a", "", "b" }, new string[] { "a", "b" }, TestName = "Skip empty words")]
    public void SplitTextCorrect(string[] words, string[] expectedResult)
    {
        var text = string.Join(Environment.NewLine, words);
        var actualResult = splitter.Split(text).ToArray();

        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
