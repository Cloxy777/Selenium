using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Managers;

// TODO: dependency on cards of player should be here ?
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
        var playerManager = new PlayerManager(player);

        switch (damageEffect.DamageType)
        {
            case DamageType.Pure:
                playerManager.ApplyPureDamage(damageEffect.Value);
                break;
            case DamageType.Tower:
                playerManager.Player.Tower = playerManager.Player.Tower - damageEffect.Value;
                break;
            default:
                throw new NotSupportedException($"{nameof(Apply)} not support {damageEffect.DamageType} damage type.");
        }

        return playerManager;
    }

    public void ApplyPureDamage(int damage)
    {
        if (damage > Player.Wall)
        {
            var towerDamage = damage - Player.Wall;
            Player.Wall = 0;
            Player.Tower = Player.Tower - towerDamage;
            return;
        }

        Player.Wall = Player.Wall - damage;
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

    public decimal GetPower(PlayerManager enemyManager)
    {
        var resourcePower = CalculatePlayerResourcePower(ResourceType.Ore) +
            CalculatePlayerResourcePower(ResourceType.Mana) +
            CalculatePlayerResourcePower(ResourceType.Stacks);

        var productionPower = CalculatePlayerProductionPower(ResourceType.Mines) +
            CalculatePlayerProductionPower(ResourceType.Monasteries) +
            CalculatePlayerProductionPower(ResourceType.Barracks);

        var towerPower = CalculateTowerPower();

        var wallPower = CalculateWallPower(enemyManager);

        return resourcePower + productionPower + towerPower + wallPower;
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
            resourcePower = resourceValue * 1.5m;
        }

        if (resourceValue <= 15)
        {
            resourcePower = (7 * 1.5m) + ((resourceValue - 7) * 1.3m);
        }

        if (resourceValue <= 25)
        {
            resourcePower = (7 * 1.5m) + (8 * 1.3m) + (resourceValue - 15);
        }

        if (resourceValue > 25)
        {
            resourcePower = (7 * 1.5m) + (8 * 1.3m) + 10 + ((resourceValue - 25) * 0.9m);
        }

        return resourcePower;
    }

    private decimal CalculatePlayerProductionPower(ResourceType resourceType)
    {
        if (resourceType is not (ResourceType.Mines or ResourceType.Monasteries or ResourceType.Barracks))
        {
            throw new NotSupportedException($"{nameof(CalculatePlayerResourcePower)} not support {resourceType} resource type.");
        }

        var resourceValue = GetResourceValue(resourceType);

        var @base = 6m;

        if (resourceValue == 0) @base = 0m;
        if (resourceValue == 1) @base = 2m;
        if (resourceValue == 2) @base = 3.5m;
        if (resourceValue == 3) @base = 4.5m;
        if (resourceValue == 4) @base = 5.25m;
        if (resourceValue == 5) @base = 5.75m;
        if (resourceValue == 6) @base = 6.0m;

        return @base * TurnCounter.Left;
    }

    private decimal CalculateTowerPower()
    {
        if (Player.Tower <= 0)
        {
            return Player.Tower * 5m;
        }

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

        if (Player.Tower <= 50)
        {
            return (14 * 2.5m) + (11 * 2m) + (10 * 1.5m) + (5 * 2m) + ((Player.Tower - 40) * 2.5m);
        }

        return (14 * 2.5m) + (11 * 2m) + (10 * 1.5m) + (5 * 2m) + (10 * 2.5m) + ((Player.Tower - 50) * 5m);
    }

    private decimal CalculateWallPower(PlayerManager enemyManager)
    {
        var delta = Math.Max(0, Player.Wall - enemyManager.Player.Wall);
        //if (delta > 0 && delta < 8) delta = 10;
        //if (delta >= 8) delta = 0;

        if (Player.Wall <= 7)
        {
            return Player.Wall * 2m + delta;
        }

        if (Player.Wall <= 15)
        {
            return (7 * 2) + ((Player.Wall - 7) * 1.5m) + delta;
        }

        return (7 * 2m) + (8 * 1.5m) + (Player.Wall - 15) + delta;
    }

    public ResourceEffect GetActualNegativeEffect(int value, ResourceType resourceType, Side side)
    {
        if (value >= 0)
        {
            throw new ArgumentException($"Wrong value: {value}. {nameof(GetActualNegativeEffect)}");
        }

        value = Math.Abs(value);

        var resourceValue = GetResourceValue(resourceType);

        var actual = Math.Min(value, resourceValue);

        return new ResourceEffect(resourceType, actual * -1, side);

    }
}
