using Newtonsoft.Json;
using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Models;


namespace Selenium.Heroes.Common.Loaders;

public static class CardEffectsLoader
{
    private static CardEffect[] _cardEffects;

    static CardEffectsLoader()
    {
        var text = File.ReadAllText(StringConstants.ManualCardEffectsFullPath);
        var cardEffects = JsonConvert.DeserializeObject<CardEffect[]>(text) ?? throw new Exception("Saved card effects not parsed.");
        Console.WriteLine($"Saved card effects loaded. Count: {cardEffects.Length}.");

        _cardEffects = cardEffects;
    }

    public static IReadOnlyList<CardEffect> AllCardEffects => _cardEffects;
}
