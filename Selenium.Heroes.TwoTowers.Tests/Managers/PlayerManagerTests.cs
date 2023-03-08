using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Heroes.TwoTowers.Tests.Models;

[TestClass]
public class PlayerManagerTests
{
    [TestMethod]
    public void Apply_ResourceEffect_ShouldUpdatePlayerResources()
    {
        // Arrange
        var player = new Player("Player")
        {
            Mines = 5,
            Ore = 10,
            Monasteries = 2,
            Mana = 4,
            Barracks = 3,
            Stacks = 7,
            Tower = 30,
            Wall = 15
        };

        var playerManager = new PlayerManager(player);

        // Act
        playerManager = playerManager
            .Apply(new ResourceEffect(ResourceType.Mines, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Ore, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Monasteries, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Mana, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Barracks, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Stacks, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Tower, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Wall, 3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Mines, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Ore, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Monasteries, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Mana, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Barracks, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Stacks, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Tower, -3, Side.Player))
            .Apply(new ResourceEffect(ResourceType.Wall, -3, Side.Player));

        // Assert
        Assert.AreEqual(5, playerManager.Player.Mines);
        Assert.AreEqual(10, playerManager.Player.Ore);
        Assert.AreEqual(2, playerManager.Player.Monasteries);
        Assert.AreEqual(4, playerManager.Player.Mana);
        Assert.AreEqual(3, playerManager.Player.Barracks);
        Assert.AreEqual(7, playerManager.Player.Stacks);
        Assert.AreEqual(30, playerManager.Player.Tower);
        Assert.AreEqual(15, playerManager.Player.Wall);
    }

    [TestMethod]
    public void ApplyPureDamage_WallIsGreaterThanDamage_DamageAppliedToWall()
    {
        // Arrange
        var player = new Player("Player")
        {
            Wall = 5,
            Tower = 10
        };

        var playerManager = new PlayerManager(player);

        // Act
        playerManager.ApplyPureDamage(3);

        // Assert
        Assert.AreEqual(2, playerManager.Player.Wall);
        Assert.AreEqual(10, playerManager.Player.Tower);
    }

    [TestMethod]
    public void ApplyPureDamage_WallIsLessThanDamage_DamageAppliedToWallAndTower()
    {
        // Arrange
        var player = new Player("Player")
        {
            Wall = 2,
            Tower = 10
        };

        var playerManager = new PlayerManager(player);

        // Act
        playerManager.ApplyPureDamage(5);

        // Assert
        Assert.AreEqual(0, playerManager.Player.Wall);
        Assert.AreEqual(7, playerManager.Player.Tower);
    }
}
