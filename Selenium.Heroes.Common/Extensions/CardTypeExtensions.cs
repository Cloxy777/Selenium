using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Extensions;

public static class CardTypeExtensions
{
    public static ResourceType GetResourceType(this CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Ore: return ResourceType.Ore;
            case CardType.Mana: return ResourceType.Mana;
            case CardType.Stacks: return ResourceType.Stacks;

            default: throw new NotSupportedException($"{nameof(GetResourceType)} not support {cardType} card type.");
        }
    }
}
