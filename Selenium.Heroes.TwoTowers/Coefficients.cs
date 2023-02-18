using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Models;
using System;

namespace Selenium.Heroes.TwoTowers;


public static class Coefficients
{
    public static decimal GetDamageCoefficient(DamageEffect damageEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (damageEffect.DamageType)
        {
            case DamageType.Pure: 
                return GetPureDamageCoefficient(damageEffect, player, enemy, cardDescriptors);
            case DamageType.Tower: 
                return GetTowerDamageCoefficient(damageEffect, player, enemy, cardDescriptors);
            default: 
                throw new NotSupportedException($"{nameof(GetDamageCoefficient)} not support {damageEffect.DamageType} damage type.");
        }
    }

    private static decimal GetPureDamageCoefficient(DamageEffect damageEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (damageEffect.Side)
        {
            case Side.Player:
                return GetPlayerPureDamageCoefficient(damageEffect, player, cardDescriptors);
            case Side.Enemy:
                return GetEnemyPureDamageCoefficient(damageEffect, enemy, cardDescriptors);
            default: 
                throw new NotSupportedException($"{nameof(GetPureDamageCoefficient)} not support {damageEffect.Side} side.");
        }
    }

    private static decimal GetPlayerPureDamageCoefficient(DamageEffect damageEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        if (player.Wall - damageEffect.Value > 7)
        {
            return 2m;
        }

        if (player.Wall - damageEffect.Value > 0 && player.Tower > 20)
        {
            return 1.7m;
        }

        if (player.Wall - damageEffect.Value > -3 && player.Tower > 20)
        {
            return 1.5m;
        }

        if (player.Tower < 20)
        {
            return 0.5m;
        }

        return 1m;
    }

    private static decimal GetEnemyPureDamageCoefficient(DamageEffect damageEffect, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        var wallDamage = enemy.Wall - damageEffect.Value;
        var effect = wallDamage / (decimal)damageEffect.Value;

        return 1 + effect;
    }

    private static decimal GetTowerDamageCoefficient(DamageEffect damageEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (damageEffect.Side)
        {
            case Side.Player:
                return GetPlayerTowerDamageCoefficient(damageEffect, player, cardDescriptors);
            case Side.Enemy:
                return GetEnemyTowerDamageCoefficient(damageEffect, enemy, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetPureDamageCoefficient)} not support {damageEffect.Side} side.");
        }
    }

    private static decimal GetPlayerTowerDamageCoefficient(DamageEffect damageEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        if (player.Tower - damageEffect.Value > 14)
        {
            return 1m;
        }

        if (player.Tower - damageEffect.Value > 0)
        {
            return 1.5m;
        }

        return 2m;
    }

    private static decimal GetEnemyTowerDamageCoefficient(DamageEffect damageEffect, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        if (enemy.Tower - damageEffect.Value > 14)
        {
            return 1m;
        }

        if (enemy.Tower - damageEffect.Value > 0)
        {
            return 1.3m;
        }

        return 10m;
    }

    public static decimal GetResourceCoefficient(ResourceEffect resourceEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.ResourceType)
        {
            case ResourceType.Mines: 
            case ResourceType.Monasteries: 
            case ResourceType.Barracks: 
                return GetProductionCoefficient(resourceEffect, player, enemy, cardDescriptors);
            case ResourceType.Ore:
            case ResourceType.Mana:
            case ResourceType.Stacks:
                return GetCoinCoefficient(resourceEffect, player, enemy, cardDescriptors);
            case ResourceType.Tower: 
                return GetTowerCoefficient(resourceEffect, player, enemy, cardDescriptors);
            case ResourceType.Wall:
                return GetWallCoefficient(resourceEffect, player, enemy, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetResourceCoefficient)} not support {resourceEffect.ResourceType} resource type.");
        }
    }

    private static decimal GetProductionCoefficient(ResourceEffect resourceEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player:
                return GetProductionCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Enemy:
                return GetProductionCoefficient(resourceEffect, enemy, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetProductionCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetProductionCoefficient(ResourceEffect resourceEffect, Player unknown, List<ICardDescriptor> cardDescriptors)
    {
        var productionValue = resourceEffect.ResourceType.GetResourceValue(unknown);
        var resourceValue = resourceEffect.ResourceType.GetProductionDependentResourceValue(unknown);

        var coefficient = (2 - productionValue / 10m) - resourceValue / 150m;
        coefficient = coefficient < 0.5m ? 0.5m : coefficient;
        return coefficient;
    }

    private static decimal GetCoinCoefficient(ResourceEffect resourceEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player when resourceEffect.Value >= 0:
                return GetPositivePlayerCoinCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Player when resourceEffect.Value < 0:
                return GetNegativePlayerCoinCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Enemy when resourceEffect.Value >= 0:
                return 1m;
            case Side.Enemy when resourceEffect.Value < 0:
                return GetNegativeEnemyCoinCoefficient(resourceEffect, enemy, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetCoinCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetPositivePlayerCoinCoefficient(ResourceEffect resourceEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        var disabledCards = cardDescriptors.Where(x => 
            x.BaseCardEffect.Card.CardType.GetResourceType() == resourceEffect.ResourceType && 
            !x.IsEnabled(player)).ToList();

        var nextTurn = player.Apply(resourceEffect);
        var enabledCards = disabledCards.Where(x => 
            x.BaseCardEffect.Card.CardType.GetResourceType() == resourceEffect.ResourceType && 
            x.IsEnabled(nextTurn)).ToList();

        var delta = enabledCards.Count / 10m;

        return 1m + delta;
    }

    private static decimal GetNegativePlayerCoinCoefficient(ResourceEffect resourceEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        var playerResourceValue = resourceEffect.ResourceType.GetResourceValue(player);

        var resourceEffectValue = Math.Abs(resourceEffect.Value);

        var actual = Math.Min(playerResourceValue, resourceEffectValue);

        var delta = actual / resourceEffectValue;

        return 1m + delta;
    }

    private static decimal GetNegativeEnemyCoinCoefficient(ResourceEffect resourceEffect, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        var enemyResourceValue = resourceEffect.ResourceType.GetResourceValue(enemy);

        var resourceEffectValue = Math.Abs(resourceEffect.Value);

        var actual = Math.Min(enemyResourceValue, resourceEffectValue);

        var delta = actual / resourceEffectValue;

        return 0.5m + delta;
    }

    private static decimal GetTowerCoefficient(ResourceEffect resourceEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player when resourceEffect.Value >= 0:
                return GetPositivePlayerTowerCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Player when resourceEffect.Value < 0:
                return GetNegativePlayerTowerCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Enemy when resourceEffect.Value >= 0:
                return 1m;
            case Side.Enemy when resourceEffect.Value < 0:
                return GetNegativeEnemyTowerCoefficient(resourceEffect, enemy, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetTowerCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetPositivePlayerTowerCoefficient(ResourceEffect resourceEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        if (player.Tower < 50)
        {
            return 1.5m;
        }

        if (player.Tower < 40)
        {
            return 1.2m;
        }

        if (player.Tower < 35)
        {
            return 1m;
        }

        if (player.Tower < 25)
        {
            return 1.2m;
        }

        if (player.Tower < 14)
        {
            return 1.5m;
        }

        if (player.Tower < 7)
        {
            return 10m;
        }

        return 10m;
    }

    private static decimal GetNegativePlayerTowerCoefficient(ResourceEffect resourceEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        if (player.Tower < 50)
        {
            return 1m;
        }

        if (player.Tower < 14)
        {
            return 0.7m;
        }

        if (player.Tower < 7)
        {
            return 0.3m;
        }

        return 0.3m;
    }

    private static decimal GetNegativeEnemyTowerCoefficient(ResourceEffect resourceEffect, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        if (enemy.Tower < 50)
        {
            return 1m;
        }

        if (enemy.Tower < 14)
        {
            return 1.3m;
        }

        if (enemy.Tower < 7)
        {
            return 1.5m;
        }

        return 1.5m;
    }

    private static decimal GetWallCoefficient(ResourceEffect resourceEffect, Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player when resourceEffect.Value >= 0:
                return GetPositivePlayerWallCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Player when resourceEffect.Value < 0:
                return GetNegativePlayerWallCoefficient(resourceEffect, player, cardDescriptors);
            case Side.Enemy when resourceEffect.Value >= 0:
                return 1m;
            case Side.Enemy when resourceEffect.Value < 0:
                return GetNegativeEnemyWallCoefficient(resourceEffect, enemy, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetWallCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetPositivePlayerWallCoefficient(ResourceEffect resourceEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        if (player.Wall > 20)
        {
            return 1m;
        }

        if (player.Wall > 7)
        {
            return 1.1m;
        }

        return 1.2m;
    }

    private static decimal GetNegativePlayerWallCoefficient(ResourceEffect resourceEffect, Player player, List<ICardDescriptor> cardDescriptors)
    {
        if (player.Wall - Math.Abs(resourceEffect.Value) > 7)
        {
            return 1.2m;
        }

        if (player.Wall - Math.Abs(resourceEffect.Value) > 0)
        {
            return 1.1m;
        }

        return 0.9m;
    }

    private static decimal GetNegativeEnemyWallCoefficient(ResourceEffect resourceEffect, Player enemy, List<ICardDescriptor> cardDescriptors)
    {
        if (enemy.Wall - Math.Abs(resourceEffect.Value) > 0)
        {
            return 1m;
        }

        return 1.5m;
    }
}
