using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.CardDescriptors;

public interface ICardDescriptor
{
    CardEffect BaseCardEffect { get; }

    CardEffect GetActualCardEffect(Player player, Player enemy, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor);
}

public abstract class CardDescriptor : ICardDescriptor
{
    public abstract CardEffect BaseCardEffect { get; }

    public virtual CardEffect GetActualCardEffect(Player player, Player enemy, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        return BaseCardEffect;
    }
}