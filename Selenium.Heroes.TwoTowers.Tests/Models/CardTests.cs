using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers.Tests.Models;

[TestClass]
public class CardTests
{
    [TestMethod]
    public void Equals_ReturnsTrue_WhenComparedToItself()
    {
        // Arrange
        var card = new Card { Header = "Card Header", Description = "Card Description", Cost = 100, CardType = CardType.Mana };

        // Act
        var result = card.Equals(card);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_ReturnsTrue_WhenComparedToCardWithSameHeader()
    {
        // Arrange
        var card1 = new Card { Header = "Card Header", Description = "Card Description", Cost = 100, CardType = CardType.Mana };
        var card2 = new Card { Header = "Card Header", Description = "Different Description", Cost = 200, CardType = CardType.Ore };

        // Act
        var result = card1.Equals(card2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalse_WhenComparedToCardWithDifferentHeader()
    {
        // Arrange
        var card1 = new Card { Header = "Card Header", Description = "Card Description", Cost = 100, CardType = CardType.Mana };
        var card2 = new Card { Header = "Different Header", Description = "Different Description", Cost = 200, CardType = CardType.Ore };

        // Act
        var result = card1.Equals(card2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalse_WhenComparedToNull()
    {
        // Arrange
        var card = new Card { Header = "Card Header", Description = "Card Description", Cost = 100, CardType = CardType.Mana };

        // Act
        var result = card.Equals(null);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalse_WhenComparedToNonCardObject()
    {
        // Arrange
        var card = new Card { Header = "Card Header", Description = "Card Description", Cost = 100, CardType = CardType.Mana };
        var nonCardObject = "This is not a card";

        // Act
        var result = card.Equals(nonCardObject);

        // Assert
        Assert.IsFalse(result);
    }
}
