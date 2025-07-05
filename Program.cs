using MoreLinq;
using Serilog;
using System.Diagnostics;
using WeCantSpell.Hunspell;
using System.Linq;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var dictionary = WordList.CreateFromFiles("af_ZA.dic", "af_ZA.aff");

var alphabet = "abcdefghijklmnopqrstuvwxyz".Select(c => c.ToString()).ToArray();

Debug.Assert(alphabet.Length == 26);

var wordLength = 5;
log.Information("Generating all {wordLength}-letter words from the alphabet...", wordLength);

var words = GenerateAllWords(alphabet, wordLength);

var count = Math.Pow(alphabet.Length, wordLength);
log.Information("Generated {Count} words.", count);
log.Information("First 10 words: {Words}", words.Take(10));

log.Information("Checking words against the dictionary...");

foreach (var word in words.AsParallel().Where(dictionary.Check))
{
    Console.WriteLine(word);
}

IEnumerable<string> GenerateAllWords(string[] alphabet, int length)
{
    if (length < 1)
    {
        return Enumerable.Empty<string>();
    }
    else if (length == 1)
    {
        return alphabet;
    }
    else
    {
        return alphabet.Cartesian(GenerateAllWords(alphabet, length - 1), (s1, s2) => s1 + s2);
    }
}