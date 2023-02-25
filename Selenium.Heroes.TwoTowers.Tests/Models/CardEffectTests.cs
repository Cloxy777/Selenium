using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Heroes.TwoTowers.Tests.Models;

[TestClass]
public class CardEffectTests
{
    [TestMethod]
    public void Equals_ShouldReturnTrue_WhenGivenEqualObjects()
    {
        // Arrange
        var card = new Card { Header = "Test Card" };
        var resourceEffect = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var damageEffect = new DamageEffect(DamageType.Pure, 5, Side.Enemy);
        var cardEffect1 = new CardEffect
        {
            Card = card,
            ResourceEffects = new List<ResourceEffect> { resourceEffect },
            DamageEffects = new List<DamageEffect> { damageEffect },
            PlayType = PlayType.Default
        };
        var cardEffect2 = new CardEffect
        {
            Card = card,
            ResourceEffects = new List<ResourceEffect> { resourceEffect },
            DamageEffects = new List<DamageEffect> { damageEffect },
            PlayType = PlayType.Default
        };

        // Act
        var result = cardEffect1.Equals(cardEffect2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_WhenGivenObjectsWithDifferentCard()
    {
        // Arrange
        var card1 = new Card { Header = "Test Card 1" };
        var card2 = new Card { Header = "Test Card 2" };
        var resourceEffect = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var damageEffect = new DamageEffect(DamageType.Pure, 5, Side.Enemy);
        var cardEffect1 = new CardEffect
        {
            Card = card1,
            ResourceEffects = new List<ResourceEffect> { resourceEffect },
            DamageEffects = new List<DamageEffect> { damageEffect },
            PlayType = PlayType.Default
        };
        var cardEffect2 = new CardEffect
        {
            Card = card2,
            ResourceEffects = new List<ResourceEffect> { resourceEffect },
            DamageEffects = new List<DamageEffect> { damageEffect },
            PlayType = PlayType.Default
        };

        // Act
        var result = cardEffect1.Equals(cardEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_WhenGivenObjectsWithDifferentResourceEffects()
    {
        // Arrange
        var card = new Card { Header = "Test Card" };
        var resourceEffect1 = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var resourceEffect2 = new ResourceEffect(ResourceType.Mines, 5, Side.Player);
        var damageEffect = new DamageEffect(DamageType.Pure, 5, Side.Enemy);
        var cardEffect1 = new CardEffect
        {
            Card = card,
            ResourceEffects = new List<ResourceEffect> { resourceEffect1 },
            DamageEffects = new List<DamageEffect> { damageEffect },
            PlayType = PlayType.Default
        };
        var cardEffect2 = new CardEffect
        {
            Card = card,
            ResourceEffects = new List<ResourceEffect> { resourceEffect2 },
            DamageEffects = new List<DamageEffect> { damageEffect },
            PlayType = PlayType.Default
        };

        // Act
        var result = cardEffect1.Equals(cardEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CardEffect_Equals_ReturnsFalse_WhenPlayTypeIsDifferent()
    {
        // Arrange
        var cardEffect1 = new CardEffect
        {
            Card = new Card { Header = "Card A" },
            ResourceEffects = new List<ResourceEffect>(),
            DamageEffects = new List<DamageEffect>(),
            PlayType = PlayType.Default
        };
        var cardEffect2 = new CardEffect
        {
            Card = new Card { Header = "Card A" },
            ResourceEffects = new List<ResourceEffect>(),
            DamageEffects = new List<DamageEffect>(),
            PlayType = PlayType.PlayAgain
        };

        // Act
        var result = cardEffect1.Equals(cardEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CardEffect_Equals_ReturnsFalse_WhenResourceEffectsAreDifferent()
    {
        // Arrange
        var cardEffect1 = new CardEffect
        {
            Card = new Card { Header = "Card A" },
            ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, 1, Side.Player),
            new ResourceEffect(ResourceType.Monasteries, 2, Side.Enemy)
        },
            DamageEffects = new List<DamageEffect>(),
            PlayType = PlayType.Default
        };
        var cardEffect2 = new CardEffect
        {
            Card = new Card { Header = "Card A" },
            ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, 1, Side.Player),
            new ResourceEffect(ResourceType.Monasteries, 3, Side.Enemy)
        },
            DamageEffects = new List<DamageEffect>(),
            PlayType = PlayType.Default
        };

        // Act
        var result = cardEffect1.Equals(cardEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CardEffect_Equals_ReturnsFalse_WhenDamageEffectsAreDifferent()
    {
        // Arrange
        var cardEffect1 = new CardEffect
        {
            Card = new Card { Header = "Card A" },
            ResourceEffects = new List<ResourceEffect>(),
            DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 1, Side.Player),
            new DamageEffect(DamageType.Tower, 2, Side.Enemy)
        },
            PlayType = PlayType.Default
        };
        var cardEffect2 = new CardEffect
        {
            Card = new Card { Header = "Card A" },
            ResourceEffects = new List<ResourceEffect>(),
            DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 1, Side.Player),
            new DamageEffect(DamageType.Tower, 3, Side.Enemy)
        },
            PlayType = PlayType.Default
        };

        // Act
        var result = cardEffect1.Equals(cardEffect2);

        // Assert
        Assert.IsFalse(result);
    }
}
