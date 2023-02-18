using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Selenium.Heroes.TwoTowers;


// TODO:
// 1. Test correct weights. In plays =)
// 2. Understand when we play when we discard
// 3. Understand when we play weak and need to discard
// 4. Understand combinations


// Come up with some strategies to calculate card`s weight if another card played and operate by combinations inside
public class DecisionMaker
{
    private const decimal WEIGHT_THRESHOLD = 0.3m;

    public DecisionMaker(Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
	{
		Player = player; 
		Enemy = enemy; 
		CardDescriptors = cardDescriptors;
	}

	public Player Player { get; }

    public Player Enemy { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public Decision MakeDecision()
    {
        var canPlay = CardDescriptors.Any(x => x.IsEnabled(Player));
        var actionType = canPlay ? ActionType.Play : ActionType.Discard;

        if (canPlay)
        {
            (var weight, var cardDescriptor) = GetMostEffectiveCardDescriptor();

            if (weight > WEIGHT_THRESHOLD)
            {
                var decision = new Decision
                {
                    ActionType = ActionType.Play,
                    CardDescriptor = cardDescriptor
                };
                return decision;
            }
        }

        return new Decision
        {
            ActionType = ActionType.Discard,
            CardDescriptor = GetCardDescriptorToDiscard(),
        };
    }

    private ICardDescriptor GetCardDescriptorToDiscard()
    {
        var future = Player.Wait().Wait().Wait();

        return CardDescriptors
            .Where(x => !x.IsEnabled(future))
            .OrderByDescending(x => x.ResourceLackNumber(future))
            .First();
    }

    public (decimal Weight, ICardDescriptor CardDescriptor) GetMostEffectiveCardDescriptor()
    {
        var cardWights = new List<(decimal Weight, ICardDescriptor CardDescriptor)>();
        foreach (var cardDescriptor in CardDescriptors)
        {
            var weight = CalculateWeight(Player, Enemy, CardDescriptors, cardDescriptor);

            var header = cardDescriptor.BaseCardEffect.Card.Header;

            if (cardDescriptor.BaseCardEffect.PlayType == PlayType.PlayAgain)
            {
                weight += 1;
            }

            Console.WriteLine($"{header} = {weight}.");
            cardWights.Add(new (weight, cardDescriptor));
        }

        return cardWights.Where(x => x.CardDescriptor.IsEnabled(Player)).MaxBy(x => x.Weight);
    }

    private decimal CalculateWeight(Player player, Player enemy, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var card = cardDescriptor.BaseCardEffect.Card;     
        var cost = card.TransformedCost;

        var baseCardEffect = cardDescriptor.BaseCardEffect;
        var actualCardEffect = cardDescriptor.GetActualCardEffect(player, enemy, cardDescriptors, cardDescriptor);
        var conditionWeightCoefficient = !actualCardEffect.Equals(baseCardEffect) ? 1.1m : 1;

        var resourceWeight = CalculateWeight(player, enemy, actualCardEffect.ResourceEffects);
        var damageWeight = CalculateWeight(player, enemy, actualCardEffect.DamageEffects);
        var isPlayAgain = actualCardEffect.PlayType == PlayType.PlayAgain;
        var playAgainCoefficient = isPlayAgain ? 1.1m : 1m;

        var weight = (resourceWeight + damageWeight - cost) * conditionWeightCoefficient * playAgainCoefficient;

        return Math.Round(weight, 2);
    }

    private decimal CalculateWeight(Player player, Player enemy, List<ResourceEffect> resourceEffects)
    {
        var weight = 0m;

        foreach (var resourceEffect in resourceEffects)
        {
            weight += CalculateWeight(player, enemy, resourceEffect);
        }

        return weight;
    }

    private decimal CalculateWeight(Player player, Player enemy, ResourceEffect resourceEffect)
    {
        var value = resourceEffect.TransformedValue;
        var invert = resourceEffect.Side == Side.Player ? 1 : -1;

        var resourceCoefficient = BasePowerCoefficients.GetResourceCoefficient(resourceEffect, Player, Enemy, CardDescriptors);

        var weight = value * resourceCoefficient * invert;

        return weight;
    }

    private decimal CalculateWeight(Player player, Player enemy, List<DamageEffect> damageEffects)
    {
        var weight = 0m;

        foreach (var damageEffect in damageEffects)
        {
            weight += CalculateWeight(player, enemy, damageEffect);
        }

        return weight;
    }

    private decimal CalculateWeight(Player player, Player enemy, DamageEffect damageEffect)
    {
        var value = damageEffect.TransformedValue;
        var invert = damageEffect.Side == Side.Player ? -1 : 1;

        var damageCoefficient = BasePowerCoefficients.GetDamageCoefficient(damageEffect, Player, Enemy, CardDescriptors);

        var weight = value * damageCoefficient * invert;

        return weight;
    }
}
