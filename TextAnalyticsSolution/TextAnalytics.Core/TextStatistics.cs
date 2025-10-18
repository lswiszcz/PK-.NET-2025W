namespace TextAnalytics.Core;

public sealed record TextStatistics(
    int CharactersWithSpaces,
    int CharactersWithoutSpaces,
    int Letters,
    int Digits,
    int Punctuation,
    int WordCount,
    int UniqueWordCount,
    string MostCommonWord,
    double AverageWordLength,
    string LongestWord,
    string ShortestWord,
    int SentenceCount,
    double AverageWordsPerSentence,
    string LongestSentence
);