using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.CardDescriptors;

public interface ICardDescriptor
{
    CardEffect BaseCardEffect { get; }

    CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor);
}

public abstract class CardDescriptor : ICardDescriptor
{
    public abstract CardEffect BaseCardEffect { get; }

    public virtual CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        return BaseCardEffect;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CardDescriptor)
        {
            return Equals((CardDescriptor)obj);
        }

        return false;
    }

    public bool Equals(CardDescriptor cardDescriptor)
    {
        return BaseCardEffect.Card.Header == cardDescriptor.BaseCardEffect.Card.Header;
    }

    public override int GetHashCode()
    {
        return BaseCardEffect.Card.Header.GetHashCode();
    }

    protected virtual ResourceEffect GetActualNegativeEffect(PlayerManager playerManager, PlayerManager enemyManager, int value, ResourceType resourceType, Side side)
    {
        switch (side)
        {
            case Side.Player:
                return playerManager.GetActualNegativeEffect(value, resourceType, side);
            case Side.Enemy:
                return enemyManager.GetActualNegativeEffect(value, resourceType, side);
            default:
                throw new NotSupportedException($"{nameof(GetActualNegativeEffect)} not support {side} side.");
        }
    }
}