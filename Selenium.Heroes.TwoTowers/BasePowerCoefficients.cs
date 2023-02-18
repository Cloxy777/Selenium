using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;
using System;

namespace Selenium.Heroes.TwoTowers;


public static class BasePowerCoefficients
{
    public static decimal GetDamageCoefficient(DamageEffect damageEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (damageEffect.DamageType)
        {
            case DamageType.Pure: 
                return GetPureDamageCoefficient(damageEffect, playerManager, enemyManager, cardDescriptors);
            case DamageType.Tower: 
                return GetTowerDamageCoefficient(damageEffect, playerManager, enemyManager, cardDescriptors);
            default: 
                throw new NotSupportedException($"{nameof(GetDamageCoefficient)} not support {damageEffect.DamageType} damage type.");
        }
    }

    private static decimal GetPureDamageCoefficient(DamageEffect damageEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (damageEffect.Side)
        {
            case Side.Player:
                return GetPlayerPureDamageCoefficient(damageEffect, playerManager, cardDescriptors);
            case Side.Enemy:
                return GetEnemyPureDamageCoefficient(damageEffect, enemyManager, cardDescriptors);
            default: 
                throw new NotSupportedException($"{nameof(GetPureDamageCoefficient)} not support {damageEffect.Side} side.");
        }
    }

    private static decimal GetPlayerPureDamageCoefficient(DamageEffect damageEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        if (playerManager.Player.Wall - damageEffect.Value > 7)
        {
            return 2m;
        }

        if (playerManager.Player.Wall - damageEffect.Value > 0 && playerManager.Player.Tower > 20)
        {
            return 1.7m;
        }

        if (playerManager.Player.Wall - damageEffect.Value > -3 && playerManager.Player.Tower > 20)
        {
            return 1.5m;
        }

        if (playerManager.Player.Tower < 20)
        {
            return 0.5m;
        }

        return 1m;
    }

    private static decimal GetEnemyPureDamageCoefficient(DamageEffect damageEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        var wallDamage = playerManager.Player.Wall - damageEffect.Value;
        var effect = wallDamage / (decimal)damageEffect.Value;

        return 1 + effect;
    }

    private static decimal GetTowerDamageCoefficient(DamageEffect damageEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (damageEffect.Side)
        {
            case Side.Player:
                return GetPlayerTowerDamageCoefficient(damageEffect, playerManager, cardDescriptors);
            case Side.Enemy:
                return GetEnemyTowerDamageCoefficient(damageEffect, enemyManager, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetPureDamageCoefficient)} not support {damageEffect.Side} side.");
        }
    }

    private static decimal GetPlayerTowerDamageCoefficient(DamageEffect damageEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        if (playerManager.Player.Tower - damageEffect.Value > 14)
        {
            return 1m;
        }

        if (playerManager.Player.Tower - damageEffect.Value > 0)
        {
            return 1.5m;
        }

        return 2m;
    }

    private static decimal GetEnemyTowerDamageCoefficient(DamageEffect damageEffect, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        if (enemyManager.Player.Tower - damageEffect.Value > 14)
        {
            return 1m;
        }

        if (enemyManager.Player.Tower - damageEffect.Value > 0)
        {
            return 1.3m;
        }

        return 10m;
    }

    public static decimal GetResourceCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.ResourceType)
        {
            case ResourceType.Mines: 
            case ResourceType.Monasteries: 
            case ResourceType.Barracks: 
                return GetProductionCoefficient(resourceEffect, playerManager, enemyManager, cardDescriptors);
            case ResourceType.Ore:
            case ResourceType.Mana:
            case ResourceType.Stacks:
                return GetCoinCoefficient(resourceEffect, playerManager, enemyManager, cardDescriptors);
            case ResourceType.Tower: 
                return GetTowerCoefficient(resourceEffect, playerManager, enemyManager, cardDescriptors);
            case ResourceType.Wall:
                return GetWallCoefficient(resourceEffect, playerManager, enemyManager, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetResourceCoefficient)} not support {resourceEffect.ResourceType} resource type.");
        }
    }

    private static decimal GetProductionCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player:
                return GetProductionCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Enemy:
                return GetProductionCoefficient(resourceEffect, enemyManager, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetProductionCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetProductionCoefficient(ResourceEffect resourceEffect, PlayerManager unknownManager, List<ICardDescriptor> cardDescriptors)
    {
        var productionValue = unknownManager.GetResourceValue(resourceEffect.ResourceType);

        var producedResourceType = resourceEffect.ResourceType.ToProducedType();
        var resourceValue = unknownManager.GetResourceValue(producedResourceType);

        var coefficient = (2 - productionValue / 10m) - resourceValue / 150m;
        coefficient = coefficient < 0.5m ? 0.5m : coefficient;
        return coefficient;
    }

    private static decimal GetCoinCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player when resourceEffect.Value >= 0:
                return GetPositivePlayerCoinCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Player when resourceEffect.Value < 0:
                return GetNegativePlayerCoinCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Enemy when resourceEffect.Value >= 0:
                return 1m;
            case Side.Enemy when resourceEffect.Value < 0:
                return GetNegativeEnemyCoinCoefficient(resourceEffect, enemyManager, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetCoinCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetPositivePlayerCoinCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        var disabledCards = cardDescriptors.Where(x => 
            x.BaseCardEffect.Card.CardType.GetResourceType() == resourceEffect.ResourceType && 
            !x.IsEnabled(playerManager)).ToList();

        var productionResourceType = resourceEffect.ResourceType.ToProductionType();
        var nextTurn = playerManager.Apply(resourceEffect).Produce(productionResourceType);
        var enabledCards = disabledCards.Where(x => 
            x.BaseCardEffect.Card.CardType.GetResourceType() == resourceEffect.ResourceType && 
            x.IsEnabled(nextTurn)).ToList();

        var delta = enabledCards.Count / 10m;

        return 1m + delta;
    }

    private static decimal GetNegativePlayerCoinCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        var productionType = resourceEffect.ResourceType.ToProductionType();
        var playerResourceProductionValue = playerManager.GetResourceValue(productionType);

        var playerResourceValue = playerManager.GetResourceValue(resourceEffect.ResourceType);

        var resourceEffectValue = Math.Abs(resourceEffect.Value);

        var actual = Math.Min(playerResourceValue, resourceEffectValue);
        var delta = (actual / (decimal)resourceEffectValue);

        var enabledCards = cardDescriptors.Where(x =>
            x.BaseCardEffect.Card.CardType.GetResourceType() == resourceEffect.ResourceType &&
            x.IsEnabled(playerManager)).ToList();

        var nextTurn = playerManager.Apply(resourceEffect);
        var disabledCards = enabledCards.Where(x =>
            x.BaseCardEffect.Card.CardType.GetResourceType() == resourceEffect.ResourceType &&
            !x.IsEnabled(nextTurn)).ToList();

        delta += disabledCards.Count / 10m;

        return 1m + delta;
    }

    private static decimal GetNegativeEnemyCoinCoefficient(ResourceEffect resourceEffect, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        var enemyResourceValue = enemyManager.GetResourceValue(resourceEffect.ResourceType);

        var resourceEffectValue = Math.Abs(resourceEffect.Value);

        var actual = Math.Min(enemyResourceValue, resourceEffectValue);

        var delta = actual / resourceEffectValue;

        return 0.5m + delta;
    }

    private static decimal GetTowerCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player when resourceEffect.Value >= 0:
                return GetPositivePlayerTowerCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Player when resourceEffect.Value < 0:
                return GetNegativePlayerTowerCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Enemy when resourceEffect.Value >= 0:
                return 1m;
            case Side.Enemy when resourceEffect.Value < 0:
                return GetNegativeEnemyTowerCoefficient(resourceEffect, enemyManager, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetTowerCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetPositivePlayerTowerCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        if (playerManager.Player.Tower < 50)
        {
            return 1.5m;
        }

        if (playerManager.Player.Tower < 40)
        {
            return 1.2m;
        }

        if (playerManager.Player.Tower < 35)
        {
            return 1m;
        }

        if (playerManager.Player.Tower < 25)
        {
            return 1.2m;
        }

        if (playerManager.Player.Tower < 14)
        {
            return 1.5m;
        }

        if (playerManager.Player.Tower < 7)
        {
            return 10m;
        }

        return 10m;
    }

    private static decimal GetNegativePlayerTowerCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        if (playerManager.Player.Tower < 50)
        {
            return 1m;
        }

        if (playerManager.Player.Tower < 14)
        {
            return 0.7m;
        }

        if (playerManager.Player.Tower < 7)
        {
            return 0.3m;
        }

        return 0.3m;
    }

    private static decimal GetNegativeEnemyTowerCoefficient(ResourceEffect resourceEffect, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        if (enemyManager.Player.Tower < 50)
        {
            return 1m;
        }

        if (enemyManager.Player.Tower < 14)
        {
            return 1.3m;
        }

        if (enemyManager.Player.Tower < 7)
        {
            return 1.5m;
        }

        return 1.5m;
    }

    private static decimal GetWallCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        switch (resourceEffect.Side)
        {
            case Side.Player when resourceEffect.Value >= 0:
                return GetPositivePlayerWallCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Player when resourceEffect.Value < 0:
                return GetNegativePlayerWallCoefficient(resourceEffect, playerManager, cardDescriptors);
            case Side.Enemy when resourceEffect.Value >= 0:
                return 1m;
            case Side.Enemy when resourceEffect.Value < 0:
                return GetNegativeEnemyWallCoefficient(resourceEffect, enemyManager, cardDescriptors);
            default:
                throw new NotSupportedException($"{nameof(GetWallCoefficient)} not support {resourceEffect.Side} side.");
        }
    }

    private static decimal GetPositivePlayerWallCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        if (playerManager.Player.Wall > 20)
        {
            return 1m;
        }

        if (playerManager.Player.Wall > 7)
        {
            return 1.1m;
        }

        return 1.2m;
    }

    private static decimal GetNegativePlayerWallCoefficient(ResourceEffect resourceEffect, PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        if (playerManager.Player.Wall - Math.Abs(resourceEffect.Value) > 7)
        {
            return 1.2m;
        }

        if (playerManager.Player.Wall - Math.Abs(resourceEffect.Value) > 0)
        {
            return 1.1m;
        }

        return 0.9m;
    }

    private static decimal GetNegativeEnemyWallCoefficient(ResourceEffect resourceEffect, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        if (enemyManager.Player.Wall - Math.Abs(resourceEffect.Value) > 0)
        {
            return 1m;
        }

        return 1.5m;
    }
}
