using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Extensions;

public static class CardDescriptorExtensions
{
    public static bool IsEnabled(this ICardDescriptor cardDescriptor, Player player)
    {
        var cardType = cardDescriptor.BaseCardEffect.Card.CardType;
        var resourceType = cardType.GetResourceType();
        var resourceValue = player.GetResourceValue(resourceType);
        return cardDescriptor.BaseCardEffect.Card.Cost <= resourceValue;
    }

    public static int ResourceLackNumber(this ICardDescriptor cardDescriptor, Player player)
    {
        var cost = cardDescriptor.BaseCardEffect.Card.Cost;

        var resourceType = cardDescriptor.BaseCardEffect.Card.CardType.GetResourceType();
        var resourceValue = player.GetResourceValue(resourceType);

        return cost - resourceValue;
    }
}
