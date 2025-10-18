using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TextAnalytics.Core;
using TextAnalytics.Services;

var services = new ServiceCollection()
    .AddSingleton<ILoggerService, ConsoleLogger>()
    .AddSingleton<IInputProvider, ConsoleInputProvider>()
    .AddSingleton<TextAnalyzer>()
    .BuildServiceProvider();

var logger = services.GetRequiredService<ILoggerService>();
var analyzer = services.GetRequiredService<TextAnalyzer>();
var consoleInput = services.GetRequiredService<IInputProvider>();

logger.Log("Aplikacja uruchomiona.");

string text = string.Empty;

if (args.Length >= 2 && args[0] == "--file")
{
    var filePath = args[1];
    if (!File.Exists(filePath))
    {
        logger.Log($"Plik '{filePath}' nie istnieje!");
        return;
    }

    text = File.ReadAllText(filePath);
    logger.Log($"Wczytano tekst z pliku: {filePath}");
}
else if (args.Length >= 1 && args[0] == "--interactive")
{
    logger.Log("Tryb interaktywny (wczytywanie z klawiatury).");
    text = consoleInput.Read();
}
else
{
    logger.Log("Wpisz tekst do analizy (zatwierdź Enterem):");
    text = Console.ReadLine() ?? "";
}

if (string.IsNullOrWhiteSpace(text))
{
    logger.Log("Brak tekstu do analizy!");
    return;
}

var stats = analyzer.Analyze(text);

Console.WriteLine("\n=== WYNIKI ANALIZY ===");
Console.WriteLine($"Znaki (ze spacjami): {stats.CharactersWithSpaces}");
Console.WriteLine($"Znaki (bez spacji): {stats.CharactersWithoutSpaces}");
Console.WriteLine($"Słowa: {stats.WordCount}");
Console.WriteLine($"Unikalne słowa: {stats.UniqueWordCount}");
Console.WriteLine($"Najczęstsze słowo: {stats.MostCommonWord}");
Console.WriteLine($"Średnia długość słowa: {stats.AverageWordLength:F2}");
Console.WriteLine($"Najdłuższe słowo: {stats.LongestWord}");
Console.WriteLine($"Najkrótsze słowo: {stats.ShortestWord}");
Console.WriteLine($"Liczba zdań: {stats.SentenceCount}");
Console.WriteLine($"Średnia liczba słów/zdanie: {stats.AverageWordsPerSentence:F2}");

var json = JsonConvert.SerializeObject(stats, Formatting.Indented);
File.WriteAllText("results.json", json);
logger.Log("Wyniki zapisane do results.json");
