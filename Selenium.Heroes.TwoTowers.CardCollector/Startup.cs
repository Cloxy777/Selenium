using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Loaders;

namespace Selenium.Heroes.CardCollector;

public class Startup
{
    public static void Run() 
    {
        var engine = new HeroesCardsEngine();

        int seconds = 5;
        while (true)
        {
            var isCardGamePage = engine.IsCardGamePage();

            if (!isCardGamePage)
            {
                Console.WriteLine("Not in the card game.");
                seconds = 5;
                Thread.Sleep(seconds * 1000);
                continue;
            }

            var savedCards = CardLoader.AllCards.ToList();

            var gameCards = engine.GetAllCards();
            Console.WriteLine($"Game cards loaded. Count: {gameCards.Count}.");

            foreach (var gameCard in gameCards)
            {
                savedCards.Remove(gameCard);
                savedCards.Add(gameCard);
            }

            var cards = savedCards.OrderBy(x => x.CardType).ThenBy(x => x.Cost).ThenBy(x => x.Header).ToList();

            var settings = new JsonSerializerSettings
            {
                Converters = { new StringEnumConverter() }
            };
            var json = JsonConvert.SerializeObject(cards, Formatting.Indented, settings);
            File.WriteAllText(StringConstants.CardsFullPath, json);
            Console.WriteLine($"Saved cards overwritten.");

            seconds = 2;
            Thread.Sleep(seconds * 1000);
        }     
    }
}
