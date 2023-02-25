using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Loaders;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common;

public class DrawCard
{
    public ICardDescriptor CardDescriptor { get; set; } = default!;

    public int Age { get; set; }
}

// TODO: 
public static class Deck
{
    private static List<ICardDescriptor>? _leftCards;

    private static List<DrawCard> _drawCards = new List<DrawCard>();

    public static IReadOnlyList<DrawCard> DrawCards => _drawCards;

    public static int MaxAge { get; set; } = 15;

    public static IReadOnlyList<ICardDescriptor> LeftCards => _leftCards ?? (_leftCards = GetLeftCards());

    private static List<ICardDescriptor> GetLeftCards()
    {
        return CardDescriptorsLoader.AllCardDescriptors.Where(card => !DrawCards.Any(x => x.CardDescriptor.Equals(card))).ToList();
    }

    public static void Add(ICardDescriptor card)
    {
        Add(new List<ICardDescriptor> { card });
    }

    public static void Add(List<ICardDescriptor> cards)
    {      
        cards = Exists(cards).ToList();

        _drawCards.ForEach(x => x.Age++);
        _drawCards = _drawCards.Where(x => x.Age < MaxAge).ToList();

        var drawCards = cards.Select(card => new DrawCard { Age = 0, CardDescriptor = card }).ToList();

        var otherDrawCards = Except(drawCards).ToList();

        _drawCards.Clear();
        _drawCards.AddRange(otherDrawCards);
        _drawCards.AddRange(drawCards);

        _leftCards = null;
    }

    public static IEnumerable<DrawCard> Except(List<DrawCard> drawCards)
    {
        return _drawCards.Where(x => !drawCards.Any(y => y.CardDescriptor.Equals(x.CardDescriptor)));
    }

    public static IEnumerable<ICardDescriptor> Exists(List<ICardDescriptor> cards)
    {
        var allCards = CardDescriptorsLoader.AllCardDescriptors;
        return cards.Where(x => allCards.Any(y => y.Equals(x)));
    }

    public static decimal MaxDamage(DamageType damageType, PlayerManager playerManager)
    {
        int SumDamage(ICardDescriptor? cardDescriptor)
        {
            if (cardDescriptor == null)
            {
                return 0;
            }

            var damageEffects = cardDescriptor.BaseCardEffect.DamageEffects
                .Where(x => 
                    x.Side == Side.Enemy && 
                    x.DamageType == damageType);

            if (damageEffects.Any())
            {
                return damageEffects.Sum(x => x.Value);
            }

            return 0;
        }

        var enabledCards = LeftCards.Where(x => x.IsEnabled(playerManager));

        var cardDescriptor = enabledCards.DefaultIfEmpty().MaxBy(x => SumDamage(x));
        return SumDamage(cardDescriptor);
    }

    public static decimal AvgDamage(DamageType damageType, PlayerManager playerManager)
    {
        int SumDamage(ICardDescriptor? cardDescriptor)
        {
            if (cardDescriptor == null)
            {
                return 0;
            }

            var damageEffects = cardDescriptor.BaseCardEffect.DamageEffects
                .Where(x =>
                    x.Side == Side.Enemy &&
                    x.DamageType == damageType);

            if (damageEffects.Any())
            {
                return damageEffects.Sum(x => x.Value);
            }

            return 0;
        }

        var enabledCards = LeftCards.Where(x => x.IsEnabled(playerManager));

        return (decimal)enabledCards.DefaultIfEmpty().Average(x => SumDamage(x));
    }

    public static decimal MaxEnemyResourceEffect(ResourceType resourceType, PlayerManager playerManager)
    {
        int SumDamage(ICardDescriptor? cardDescriptor)
        {
            if (cardDescriptor == null)
            {
                return 0;
            }

            var resourceEffect = cardDescriptor.BaseCardEffect.ResourceEffects
                .Where(x =>
                    x.Side == Side.Enemy &&
                    x.ResourceType == resourceType &&
                    x.Value < 0);

            if (resourceEffect.Any())
            {
                return resourceEffect.Sum(x => x.Value);
            }

            return 0;
        }

        var enabledCards = LeftCards.Where(x => x.IsEnabled(playerManager));

        var cardDescriptor = enabledCards.DefaultIfEmpty().MinBy(x => SumDamage(x));
        return SumDamage(cardDescriptor);
    }

    public static decimal AverageEnemyResourceEffect(ResourceType resourceType, PlayerManager playerManager)
    {
        int SumDamage(ICardDescriptor? cardDescriptor)
        {
            if (cardDescriptor == null)
            {
                return 0;
            }

            var resourceEffect = cardDescriptor.BaseCardEffect.ResourceEffects
                .Where(x =>
                    x.Side == Side.Enemy &&
                    x.ResourceType == resourceType &&
                    x.Value < 0);

            if (resourceEffect.Any())
            {
                return resourceEffect.Sum(x => x.Value);
            }

            return 0;
        }

        var enabledCards = LeftCards.Where(x => x.IsEnabled(playerManager));

        return (decimal)enabledCards.DefaultIfEmpty().Average(x => SumDamage(x));
    }

    public static decimal MaxPlayerResourceEffect(ResourceType resourceType, PlayerManager playerManager)
    {
        int SumDamage(ICardDescriptor? cardDescriptor)
        {
            if (cardDescriptor == null)
            {
                return 0;
            }

            var resourceEffect = cardDescriptor.BaseCardEffect.ResourceEffects
                .Where(x =>
                    x.Side == Side.Player &&
                    x.ResourceType == resourceType);

            if (resourceEffect.Any())
            {
                return resourceEffect.Sum(x => x.Value);
            }

            return 0;
        }

        var enabledCards = LeftCards.Where(x => x.IsEnabled(playerManager));

        var cardDescriptor = enabledCards.DefaultIfEmpty().MaxBy(x => SumDamage(x));
        return SumDamage(cardDescriptor);
    }

    public static decimal AveragePlayerResourceEffect(ResourceType resourceType, PlayerManager playerManager)
    {
        int SumDamage(ICardDescriptor? cardDescriptor)
        {
            if (cardDescriptor == null)
            {
                return 0;
            }

            var resourceEffect = cardDescriptor.BaseCardEffect.ResourceEffects
                .Where(x =>
                    x.Side == Side.Player &&
                    x.ResourceType == resourceType);

            if (resourceEffect.Any())
            {
                return resourceEffect.Sum(x => x.Value);
            }

            return 0;
        }

        var enabledCards = LeftCards.Where(x => x.IsEnabled(playerManager));

        return (decimal)enabledCards.DefaultIfEmpty().Average(x => SumDamage(x));
    }

}
