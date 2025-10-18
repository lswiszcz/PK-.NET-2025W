using NUnit.Framework;
using TextAnalytics.Core;

namespace TextAnalytics.Tests;

[TestFixture]
public class TextAnalyzerTests
{
    private TextAnalyzer _analyzer = null!;

    [SetUp]
    public void Setup()
    {
        _analyzer = new TextAnalyzer();
    }

    [Test]
    public void CountWords_Returns2_ForHelloWorld()
    {
        var result = _analyzer.CountWords("Hello world!");
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void CountWords_Returns0_ForEmptyString()
    {
        var result = _analyzer.CountWords("");
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Analyze_ReturnsCorrectWordCount()
    {
        var stats = _analyzer.Analyze("Ala ma kota");
        Assert.That(stats.WordCount, Is.EqualTo(3));
    }

    [Test]
    public void Analyze_ReturnsMostCommonWord_WhenWordRepeats()
    {
        var stats = _analyzer.Analyze("kot kot pies");
        Assert.That(stats.MostCommonWord, Is.EqualTo("kot"));
    }

    [Test]
    public void Analyze_CorrectlyCountsSentences()
    {
        var stats = _analyzer.Analyze("Ala ma kota. Kot śpi! To koniec?");
        Assert.That(stats.SentenceCount, Is.EqualTo(3));
    }

    [Test]
    public void Analyze_ReturnsZeroValues_ForWhitespaceOnly()
    {
        var stats = _analyzer.Analyze("     ");
        Assert.That(stats.WordCount, Is.EqualTo(0));
        Assert.That(stats.SentenceCount, Is.EqualTo(0));
    }

    [Test]
    public void Analyze_CalculatesAverageWordLength()
    {
        var stats = _analyzer.Analyze("Ala ma kota");
        Assert.That(stats.AverageWordLength, Is.GreaterThan(2.0).And.LessThan(4.0));
    }

    [Test]
    public void Analyze_DetectsLongestAndShortestWord()
    {
        var stats = _analyzer.Analyze("siema test bardzoooo");
        Assert.That(stats.LongestWord, Is.EqualTo("bardzoooo"));
        Assert.That(stats.ShortestWord, Is.EqualTo("test"));
    }
}