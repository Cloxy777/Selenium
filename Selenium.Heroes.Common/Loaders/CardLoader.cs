using Newtonsoft.Json;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Loaders;

public static class CardLoader
{
    private static Card[] _cards;

    static CardLoader()
    {
        var text = File.ReadAllText(StringConstants.CardsFullPath);
        var cards = JsonConvert.DeserializeObject<Card[]>(text) ?? throw new Exception("Saved cards not parsed.");
        Console.WriteLine($"Saved cards loaded. Count: {cards.Length}.");

        _cards = cards;
    }

    public static IReadOnlyList<Card> AllCards => _cards;
}
