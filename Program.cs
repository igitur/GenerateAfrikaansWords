using System.Diagnostics;
using MoreLinq;
using WeCantSpell.Hunspell;

var dictionary = WordList.CreateFromFiles("af_ZA.dic", "af_ZA.aff");

var alphabet = "abcdefghijklmnopqrstuvwxyz".Select(c => c.ToString()).ToArray();

Debug.Assert(alphabet.Length == 26);

var words = GenerateAllWords(alphabet, 5);

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