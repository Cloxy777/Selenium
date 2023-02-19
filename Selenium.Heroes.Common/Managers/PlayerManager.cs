using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Managers;

public class PlayerManager
{
    public Player Player { get; set; }

    public PlayerManager(PlayerManager playerManager) : this(playerManager.Player)
    { }

    public PlayerManager(Player player)
    {
        Player = new Player(player);
    }

    public PlayerManager Apply(ResourceEffect resourceEffect)
    {
        var player = new Player(Player);

        switch (resourceEffect.ResourceType)
        {
            case ResourceType.Mines:
                player.Mines = player.Mines + resourceEffect.Value;
                break;
            case ResourceType.Ore:
                player.Ore = player.Ore + resourceEffect.Value;
                break;
            case ResourceType.Monasteries:
                player.Monasteries = player.Monasteries + resourceEffect.Value;
                break;
            case ResourceType.Mana:
                player.Mana = player.Mana + resourceEffect.Value;
                break;
            case ResourceType.Barracks:
                player.Barracks = player.Barracks + resourceEffect.Value;
                break;
            case ResourceType.Stacks:
                player.Stacks = player.Stacks + resourceEffect.Value;
                break;
            case ResourceType.Tower:
                player.Tower = player.Tower + resourceEffect.Value;
                break;
            case ResourceType.Wall:
                player.Wall = player.Wall + resourceEffect.Value;
                break;
            default:
                throw new NotSupportedException($"{nameof(Apply)} not support {resourceEffect.ResourceType} resource type.");
        }


        return new PlayerManager(player);
    }

    public PlayerManager Apply(DamageEffect damageEffect)
    {
        var player = new Player(Player);

        switch (damageEffect.DamageType)
        {
            case DamageType.Pure:
                player.ApplyPureDamage(damageEffect.Value);
                break;
            case DamageType.Tower:
                player.Tower = player.Tower - damageEffect.Value;
                break;
            default:
                throw new NotSupportedException($"{nameof(Apply)} not support {damageEffect.DamageType} damage type.");
        }

        return new PlayerManager(player);
    }

    public PlayerManager ApplyCosts(ICardDescriptor cardDescriptor)
    {
        var resourceType = cardDescriptor.BaseCardEffect.Card.CardType.GetResourceType();
        var resourceEffect = new ResourceEffect(resourceType, -1 * cardDescriptor.BaseCardEffect.Card.Cost, Side.Player);
        return Apply(resourceEffect);
    }

    public PlayerManager Produce(ResourceType resourceType)
    {
        var player = new Player(Player);

        switch (resourceType)
        {
            case ResourceType.Mines:
                player.Ore = player.Ore + player.Mines;
                break;
            case ResourceType.Monasteries:
                player.Mana = player.Mana + player.Monasteries;
                break;
            case ResourceType.Barracks:
                player.Stacks = player.Stacks + player.Barracks;
                break;
            default:
                throw new NotSupportedException($"{nameof(Produce)} not support {resourceType} resource type.");
        }

        return new PlayerManager(player);
    }

    public PlayerManager Wait()
    {
        var player = new Player(Player);

        player.Ore = player.Ore + player.Mines;

        player.Mana = player.Mana + player.Monasteries;

        player.Stacks = player.Stacks + player.Barracks;

        return new PlayerManager(player);
    }

    public int GetResourceValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Ore: return Player.Ore;
            case ResourceType.Mana: return Player.Mana;
            case ResourceType.Stacks: return Player.Stacks;
            case ResourceType.Mines: return Player.Mines;
            case ResourceType.Monasteries: return Player.Monasteries;
            case ResourceType.Barracks: return Player.Barracks;
            case ResourceType.Wall: return Player.Wall;
            case ResourceType.Tower: return Player.Tower;
            default: throw new NotSupportedException($"{nameof(GetResourceValue)} not support {resourceType} resource type.");
        }
    }

    public bool IsDestroed => Player.Tower <= 0;

    public bool IsWinner => Player.Tower >= 50;

    public decimal GetPower()
    {
        var resourcePower = CalculatePlayerResourcePower(ResourceType.Ore) +
            CalculatePlayerResourcePower(ResourceType.Mana) +
            CalculatePlayerResourcePower(ResourceType.Stacks);

        var productionPower = CalculatePlayerProductionPower(ResourceType.Mines) +
            CalculatePlayerProductionPower(ResourceType.Monasteries) +
            CalculatePlayerProductionPower(ResourceType.Barracks);

        var towerPower = CalculateTowerPower();

        var wallPower = CalculateWallPower();

        return resourcePower + productionPower + towerPower + wallPower;
    }

    private decimal CalculatePlayerProductionPower(ResourceType resourceType)
    {
        if (resourceType is not (ResourceType.Mines or ResourceType.Monasteries or ResourceType.Barracks))
        {
            throw new NotSupportedException($"{nameof(CalculatePlayerResourcePower)} not support {resourceType} resource type.");
        }

        var resourceValue = GetResourceValue(resourceType);
        return resourceValue * 20m;
    }

    private decimal CalculatePlayerResourcePower(ResourceType resourceType)
    {
        if (resourceType is not (ResourceType.Ore or ResourceType.Mana or ResourceType.Stacks))
        {
            throw new NotSupportedException($"{nameof(CalculatePlayerResourcePower)} not support {resourceType} resource type.");
        }

        var resourceValue = GetResourceValue(resourceType);

        var resourcePower = 0m;
        if (resourceValue <= 7)
        {
            resourcePower = resourceValue * 1.3m;
        }

        if (resourceValue <= 15)
        {
            resourcePower = (7 * 2) + ((resourceValue - 7) * 1.2m);
        }

        if (resourceValue <= 25)
        {
            resourcePower = (7 * 2m) + (8 * 1.5m) + (resourceValue - 15);
        }

        if (resourceValue > 25)
        {
            resourcePower = (7 * 2m) + (8 * 1.5m) + 10 + ((resourceValue - 25) * 0.9m);
        }

        return resourcePower;
    }

    private decimal CalculateTowerPower()
    {
        if (Player.Tower <= 14)
        {
            return Player.Tower * 2.5m;
        }

        if (Player.Tower <= 25)
        {
            return (14 * 2.5m) + ((Player.Tower - 14) * 2m);
        }

        if (Player.Tower <= 35)
        {
            return (14 * 2.5m) + (11 * 2m) + ((Player.Tower - 25) * 1.5m);
        }

        if (Player.Tower <= 40)
        {
            return (14 * 2.5m) + (11 * 2m) + (10 * 1.5m) + ((Player.Tower - 35) * 2m);
        }

        return (14 * 2.5m) + (11 * 2m) + (10 * 1.5m) + (5 * 2m) + ((Player.Tower - 40) * 2.5m);
    }

    private decimal CalculateWallPower()
    {
        if (Player.Wall <= 7)
        {
            return Player.Wall * 1.3m;
        }

        if (Player.Wall <= 15)
        {
            return (7 * 2) + ((Player.Wall - 7) * 1.2m);
        }

        return (7 * 2m) + (8 * 1.5m) + (Player.Wall - 15);
    }
}
