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
}