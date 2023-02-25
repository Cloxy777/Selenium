namespace Selenium.Heroes.Common.Models;

public class CardEffect
{
    public Card Card { get; set; } = new Card();

    public List<ResourceEffect> ResourceEffects { get; set; } = new List<ResourceEffect>();

    public List<DamageEffect> DamageEffects { get; set; } = new List<DamageEffect>();

    public PlayType PlayType { get; set; } = PlayType.Default;

    public override bool Equals(object? obj)
    {
        if (obj is CardEffect)
        {
            return Equals((CardEffect)obj);
        }

        return false;
    }

    private bool Equals(CardEffect resourceEffect)
    {
        return Card.Equals(resourceEffect.Card) && 
            ResourceEffects.All(x => resourceEffect.ResourceEffects.Contains(x)) && 
            DamageEffects.All(x => resourceEffect.DamageEffects.Contains(x)) && 
            PlayType.Equals(resourceEffect.PlayType);
    }

    public override int GetHashCode()
    {
        return Card.GetHashCode();
    }
}

public class ResourceEffect
{
    public ResourceEffect(ResourceType resourceType, int value, Side side)
    {
        ResourceType = resourceType;
        Value = value;
        Side = side;
    }

    public ResourceType ResourceType { get; set; }

    public int Value { get; set; }

    public decimal TransformedValue
    {
        get
        {
            switch (ResourceType)
            {
                case ResourceType.Mines:
                    return Value / 10m * 10;
                case ResourceType.Monasteries:
                    return Value / 10m * 10;
                case ResourceType.Barracks:
                    return Value / 10m * 10;
                case ResourceType.Ore:
                    return Value / 150m * 10;
                case ResourceType.Mana:
                    return Value / 150m * 10;
                case ResourceType.Stacks:
                    return Value / 150m * 10;
                case ResourceType.Tower:
                    return Value / 50m * 10;
                case ResourceType.Wall:
                    return Value / 100m * 10;
                default:
                    throw new NotSupportedException($"{nameof(TransformedValue)} not support {ResourceType} resource type.");
            }
        }
    }

    public Side Side { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is ResourceEffect)
        {
            return Equals((ResourceEffect)obj);
        }
        return false;
    }

    private bool Equals(ResourceEffect resourceEffect)
    {
        return ResourceType.Equals(resourceEffect.ResourceType) &&
            Value.Equals(resourceEffect.Value) && 
            Side.Equals(resourceEffect.Side);
    }

    public override int GetHashCode()
    {
        return ResourceType.GetHashCode() ^ Value.GetHashCode() ^ Side.GetHashCode();
    }
}

public class DamageEffect
{
    public DamageEffect(DamageType damageType, int value, Side side)
    {
        DamageType = damageType;
        Value = value;
        Side = side;
    }

    public DamageType DamageType { get; set; }

    public int Value { get; set; }

    public decimal TransformedValue
    {
        get
        {
            switch (DamageType)
            {
                case DamageType.Pure:
                    return Value / 100m * 10;
                case DamageType.Tower:
                    return Value / 50m * 10;
                default:
                    throw new NotSupportedException($"{nameof(TransformedValue)} not support {DamageType} damage type.");
            }
        }
    }

    public Side Side { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is DamageEffect)
        {
            return Equals((DamageEffect)obj);
        }
        return false;
    }

    private bool Equals(DamageEffect damageEffect)
    {
        return DamageType.Equals(damageEffect.DamageType) &&
            Value.Equals(damageEffect.Value) &&
            Side.Equals(damageEffect.Side);
    }

    public override int GetHashCode()
    {
        return DamageType.GetHashCode() ^ Value.GetHashCode() ^ Side.GetHashCode();
    }
}

public enum ResourceType
{
    Mines,
    Ore,
    Monasteries,
    Mana,
    Barracks,
    Stacks,
    Tower,
    Wall
}

public enum DamageType
{
    Pure,
    Tower
}

public enum Side
{
    Player,
    Enemy
}

public enum PlayType
{
    Default,
    PlayAgain,
    DrawDiscardAndPlayAgain
}