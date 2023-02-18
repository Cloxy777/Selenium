using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;

public class CardWeightCalculator
{
    public CardWeightCalculator(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        PlayerManager = playerManager;
        EnemyManager = enemyManager;
        CardDescriptors = cardDescriptors;
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public IEnumerable<CardWeight> GetCardWeights()
    {
        var cardWights = new List<CardWeight>();
        foreach (var cardDescriptor in CardDescriptors)
        {
            var weight = CalculateWeight(cardDescriptor);

            var header = cardDescriptor.BaseCardEffect.Card.Header;

            cardWights.Add(new CardWeight { Weight = weight, CardDescriptor = cardDescriptor });
        }

        foreach (var cardWight in cardWights.Where(x => x.CardDescriptor.BaseCardEffect.PlayType == PlayType.PlayAgain))
        {
            cardWight.Weight += cardWights
                .Where(x => x.CardDescriptor.BaseCardEffect.PlayType != PlayType.PlayAgain)
                .MaxBy(x => x.Weight)!.Weight;
        }

        foreach (var cardWight in cardWights)
        {
            Console.WriteLine($"{cardWight.CardDescriptor.BaseCardEffect.Card.Header} = {cardWight.Weight}.");
        }

        return cardWights;
    }

    private decimal CalculateWeight(ICardDescriptor cardDescriptor)
    {
        var card = cardDescriptor.BaseCardEffect.Card;
        var cost = card.TransformedCost;

        var actualCardEffect = cardDescriptor.GetActualCardEffect(PlayerManager, EnemyManager, CardDescriptors, cardDescriptor);

        var resourceWeight = CalculateWeight(actualCardEffect.ResourceEffects);
        var damageWeight = CalculateWeight(actualCardEffect.DamageEffects);

        var weight = (resourceWeight + damageWeight - cost);

        return Math.Round(weight, 2);
    }

    private decimal CalculateWeight(List<ResourceEffect> resourceEffects)
    {
        var weight = 0m;

        foreach (var resourceEffect in resourceEffects)
        {
            weight += CalculateWeight(resourceEffect);
        }

        return weight;
    }

    private decimal CalculateWeight(ResourceEffect resourceEffect)
    {
        var value = resourceEffect.TransformedValue;
        var invert = resourceEffect.Side == Side.Player ? 1 : -1;

        var resourceCoefficient = BasePowerCoefficients.GetResourceCoefficient(resourceEffect, PlayerManager, EnemyManager, CardDescriptors);

        var weight = value * resourceCoefficient * invert;

        return weight;
    }

    private decimal CalculateWeight(List<DamageEffect> damageEffects)
    {
        var weight = 0m;

        foreach (var damageEffect in damageEffects)
        {
            weight += CalculateWeight(damageEffect);
        }

        return weight;
    }

    private decimal CalculateWeight(DamageEffect damageEffect)
    {
        var value = damageEffect.TransformedValue;
        var invert = damageEffect.Side == Side.Player ? -1 : 1;

        var damageCoefficient = BasePowerCoefficients.GetDamageCoefficient(damageEffect, PlayerManager, EnemyManager, CardDescriptors);

        var weight = value * damageCoefficient * invert;

        return weight;
    }
}
