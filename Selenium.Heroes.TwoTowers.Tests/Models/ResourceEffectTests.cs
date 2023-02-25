using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers.Tests.Models;

[TestClass]
public class ResourceEffectTests
{
    [TestMethod]
    public void Equals_ShouldReturnTrue_WhenComparingToSameObject()
    {
        // Arrange
        var resourceEffect = new ResourceEffect(ResourceType.Mines, 10, Side.Player);

        // Act & Assert
        Assert.IsTrue(resourceEffect.Equals(resourceEffect));
    }

    [TestMethod]
    public void Equals_ShouldReturnTrue_WhenComparingToObjectWithSameValues()
    {
        // Arrange
        var resourceEffect1 = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var resourceEffect2 = new ResourceEffect(ResourceType.Mines, 10, Side.Player);

        // Act & Assert
        Assert.IsTrue(resourceEffect1.Equals(resourceEffect2));
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_WhenComparingToObjectWithDifferentResourceType()
    {
        // Arrange
        var resourceEffect1 = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var resourceEffect2 = new ResourceEffect(ResourceType.Monasteries, 10, Side.Player);

        // Act & Assert
        Assert.IsFalse(resourceEffect1.Equals(resourceEffect2));
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_WhenComparingToObjectWithDifferentValue()
    {
        // Arrange
        var resourceEffect1 = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var resourceEffect2 = new ResourceEffect(ResourceType.Mines, 20, Side.Player);

        // Act & Assert
        Assert.IsFalse(resourceEffect1.Equals(resourceEffect2));
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_WhenComparingToObjectWithDifferentSide()
    {
        // Arrange
        var resourceEffect1 = new ResourceEffect(ResourceType.Mines, 10, Side.Player);
        var resourceEffect2 = new ResourceEffect(ResourceType.Mines, 10, Side.Enemy);

        // Act & Assert
        Assert.IsFalse(resourceEffect1.Equals(resourceEffect2));
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_WhenComparingToObjectOfDifferentType()
    {
        // Arrange
        var resourceEffect = new ResourceEffect(ResourceType.Mines, 10, Side.Player);

        // Act & Assert
        Assert.IsFalse(resourceEffect.Equals("not a ResourceEffect object"));
    }
}