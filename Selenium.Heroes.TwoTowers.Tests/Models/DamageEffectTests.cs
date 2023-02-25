using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers.Tests.Models;

[TestClass]
public class DamageEffectTests
{
    [TestMethod]
    public void Equals_WithEqualObjects_ReturnsTrue()
    {
        // Arrange
        var damageEffect1 = new DamageEffect(DamageType.Pure, 100, Side.Player);
        var damageEffect2 = new DamageEffect(DamageType.Pure, 100, Side.Player);

        // Act
        var result = damageEffect1.Equals(damageEffect2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_WithDifferentDamageTypes_ReturnsFalse()
    {
        // Arrange
        var damageEffect1 = new DamageEffect(DamageType.Pure, 100, Side.Player);
        var damageEffect2 = new DamageEffect(DamageType.Tower, 100, Side.Player);

        // Act
        var result = damageEffect1.Equals(damageEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var damageEffect1 = new DamageEffect(DamageType.Pure, 100, Side.Player);
        var damageEffect2 = new DamageEffect(DamageType.Pure, 50, Side.Player);

        // Act
        var result = damageEffect1.Equals(damageEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithDifferentSides_ReturnsFalse()
    {
        // Arrange
        var damageEffect1 = new DamageEffect(DamageType.Pure, 100, Side.Player);
        var damageEffect2 = new DamageEffect(DamageType.Pure, 100, Side.Enemy);

        // Act
        var result = damageEffect1.Equals(damageEffect2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithNullObject_ReturnsFalse()
    {
        // Arrange
        var damageEffect = new DamageEffect(DamageType.Pure, 100, Side.Player);

        // Act
        var result = damageEffect.Equals(null);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithDifferentObjectType_ReturnsFalse()
    {
        // Arrange
        var damageEffect = new DamageEffect(DamageType.Pure, 100, Side.Player);
        var resourceType = ResourceType.Mines;

        // Act
        var result = damageEffect.Equals(resourceType);

        // Assert
        Assert.IsFalse(result);
    }
}