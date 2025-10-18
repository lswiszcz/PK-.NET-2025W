namespace TextAnalytics.Services;

public class ConsoleInputProvider : IInputProvider
{
    public string Read()
    {
        Console.WriteLine("Wpisz tekst do analizy (zatwierdź Enterem):");
        var text = Console.ReadLine();
        return text ?? string.Empty;
    }
}