using MoreLinq;
using Serilog;
using System.Diagnostics;
using WeCantSpell.Hunspell;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var dictionary = WordList.CreateFromFiles("af_ZA.dic", "af_ZA.aff");

var alphabet = "abcdefghijklmnopqrstuvwxyz".Select(c => c.ToString()).ToArray();

Debug.Assert(alphabet.Length == 26);

log.Information("Generating all 5-letter words from the alphabet...");

var words = GenerateAllWords(alphabet, 5);
log.Information("Generated {Count} words.", words.Length);
log.Information("First 10 words: {Words}", words.Take(10).ToArray());

log.Information("Checking words against the dictionary...");
foreach (var word in words.Where(dictionary.Check))
{
    Console.WriteLine(word);
}

string[] GenerateAllWords(string[] alphabet, int length)
{
    if (length < 1)
    {
        return Array.Empty<string>();
    }
    else if (length == 1)
    {
        return alphabet;
    }
    else
    {
        return alphabet.Cartesian(GenerateAllWords(alphabet, length - 1), (s1, s2) => s1 + s2).ToArray();
    }
}