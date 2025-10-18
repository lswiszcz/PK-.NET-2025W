using System.Text.RegularExpressions;

namespace TextAnalytics.Core;

public sealed class TextAnalyzer
{
    public TextStatistics Analyze(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new TextStatistics(0, 0, 0, 0, 0, 0, 0, "", 0, "", "", 0, 0, "");
        }
        
        int charsWithSpaces = text.Length;
        int charsWithoutSpaces = text.Count(c => !char.IsWhiteSpace(c));
        int letters = text.Count(char.IsLetter);
        int digits = text.Count(char.IsDigit);
        int punctuation = text.Count(char.IsPunctuation);
        
        var words = Regex.Matches(text.ToLower(), @"\b[\p{L}\p{N}']+\b")
                         .Select(m => m.Value)
                         .ToList();

        int wordCount = words.Count;
        int uniqueWordCount = words.Distinct().Count();

        string mostCommonWord = words
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .FirstOrDefault()?.Key ?? "";

        double averageWordLength = words.Count > 0 ? words.Average(w => w.Length) : 0;

        string longestWord = words.OrderByDescending(w => w.Length).FirstOrDefault() ?? "";
        string shortestWord = words.OrderBy(w => w.Length).FirstOrDefault() ?? "";
        
        var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+")
                             .Where(s => !string.IsNullOrWhiteSpace(s))
                             .ToList();

        int sentenceCount = sentences.Count;

        double averageWordsPerSentence = sentenceCount > 0
            ? sentences.Average(s => Regex.Matches(s, @"\b[\p{L}\p{N}']+\b").Count)
            : 0;

        string longestSentence = sentences
            .OrderByDescending(s => Regex.Matches(s, @"\b[\p{L}\p{N}']+\b").Count)
            .FirstOrDefault() ?? "";

        return new TextStatistics(
            charsWithSpaces,
            charsWithoutSpaces,
            letters,
            digits,
            punctuation,
            wordCount,
            uniqueWordCount,
            mostCommonWord,
            averageWordLength,
            longestWord,
            shortestWord,
            sentenceCount,
            averageWordsPerSentence,
            longestSentence
        );
    }
    
    public int CountCharacters(string text, bool includeSpaces = true) =>
        includeSpaces ? text.Length : text.Count(c => !char.IsWhiteSpace(c));

    public int CountWords(string text) =>
        Regex.Matches(text, @"\b[\p{L}\p{N}']+\b").Count;
}
