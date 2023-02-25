using Selenium.Heroes.Common.Loaders;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers.CardWeights.Tests;

[TestClass]
public class CardWeightTests
{
    [TestMethod]
    public void CardWeights_ShowAllWeights()
    {
        // Arrange
        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors;
        var player = new Player("Player")
        {
            Ore = 25,
            Mana = 25,
            Stacks = 25,
            Mines = 3,
            Monasteries = 3,
            Barracks = 3,
            Wall = 15,
            Tower = 25
        };

        var playerManager = new PlayerManager(player);

        var enemy = new Player("Enemy")
        {
            Ore = 25,
            Mana = 25,
            Stacks = 25,
            Mines = 3,
            Monasteries = 3,
            Barracks = 3,
            Wall = 15,
            Tower = 25
        };


        var enemyManager = new PlayerManager(enemy);

        var calculator = new CardWeightCalculator(playerManager, enemyManager, cardDescriptors.ToList());

        var cardWeights = calculator.CardWeights;

        foreach (var cardWeight in cardWeights.OrderBy(x => x.Weight))
        {
            var header = cardWeight.CardDescriptor.BaseCardEffect.Card.Header;
            Console.WriteLine($"Header: {header}. Weight: {cardWeight.Weight}.");
        }
    }
}
