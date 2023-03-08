using Newtonsoft.Json;
using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;
using Selenium.Heroes.Common.Loaders;

namespace Selenium.Heroes.TwoTowers.CardAnalysis;

public class Startup
{
    public static void CardsAnalysis()
    {
        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.ToList();
        var maxOreCost = cardDescriptors.Where(x => x.BaseCardEffect.Card.CardType == CardType.Ore).Max(x => x.BaseCardEffect.Card.Cost);
        var maxManaCost = cardDescriptors.Where(x => x.BaseCardEffect.Card.CardType == CardType.Mana).Max(x => x.BaseCardEffect.Card.Cost);
        var maxStacksCost = cardDescriptors.Where(x => x.BaseCardEffect.Card.CardType == CardType.Stacks).Max(x => x.BaseCardEffect.Card.Cost);

        Console.WriteLine($"Max ore cost: {maxOreCost}.");
        Console.WriteLine($"Max mana cost: {maxManaCost}.");
        Console.WriteLine($"Max stacks cost: {maxStacksCost}.");
        Console.WriteLine();


        var averageOreCost = cardDescriptors.Where(x => x.BaseCardEffect.Card.CardType == CardType.Ore).Average(x => x.BaseCardEffect.Card.Cost);
        var averageManaCost = cardDescriptors.Where(x => x.BaseCardEffect.Card.CardType == CardType.Mana).Average(x => x.BaseCardEffect.Card.Cost);
        var averageStacksCost = cardDescriptors.Where(x => x.BaseCardEffect.Card.CardType == CardType.Stacks).Average(x => x.BaseCardEffect.Card.Cost);

        Console.WriteLine($"Average ore cost: {averageOreCost}.");
        Console.WriteLine($"Average mana cost: {averageManaCost}.");
        Console.WriteLine($"Average stacks cost: {averageStacksCost}.");
        Console.WriteLine();

        var maxDamage = cardDescriptors.Where(x => x.BaseCardEffect.DamageEffects.Any(x => x.DamageType == DamageType.Pure && x.Side == Side.Enemy)).Max(x => x.BaseCardEffect.DamageEffects.Sum(x => x.Value));
        var maxTowerDamage = cardDescriptors.Where(x => x.BaseCardEffect.DamageEffects.Any(x => x.DamageType == DamageType.Tower && x.Side == Side.Enemy)).Max(x => x.BaseCardEffect.DamageEffects.Sum(x => x.Value));
        Console.WriteLine($"Max damage: {maxDamage}.");
        Console.WriteLine($"Max tower damage: {maxTowerDamage}.");
        Console.WriteLine();

        var averageDamage = cardDescriptors.Where(x => x.BaseCardEffect.DamageEffects.Any(x => x.DamageType == DamageType.Pure && x.Side == Side.Enemy)).Average(x => x.BaseCardEffect.DamageEffects.Sum(x => x.Value));
        var averageTowerDamage = cardDescriptors.Where(x => x.BaseCardEffect.DamageEffects.Any(x => x.DamageType == DamageType.Tower && x.Side == Side.Enemy)).Average(x => x.BaseCardEffect.DamageEffects.Sum(x => x.Value));
        Console.WriteLine($"Average damage: {averageDamage}.");
        Console.WriteLine($"Average tower damage: {averageTowerDamage}.");
        Console.WriteLine();
    }


    public static void ValidateCardDescriptors()
    {
        var cards = CardLoader.AllCards.ToList();

        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.ToList();

        var missed = cards.Where(card => !cardDescriptors.Any(x => x.BaseCardEffect.Card.Header == card.Header)).ToList();

        foreach (var item in missed)
        {
            Console.WriteLine("Missed card descriptors.");
            Console.WriteLine(item.Header);
        }

        Console.WriteLine("CHECK CARD CONTENT");
        foreach (var card in cards)
        {
            var descriptor = cardDescriptors.FirstOrDefault(x => x.BaseCardEffect.Card.Header == card.Header);

            if (descriptor != null)
            {
                var target = descriptor.BaseCardEffect.Card;
                if (target.CardType != card.CardType)
                {
                    Console.WriteLine($"{card.Header} wrong {nameof(card.CardType)}.");
                }

                if (target.Description != card.Description)
                {
                    Console.WriteLine($"{card.Header} wrong {nameof(card.Description)}.");
                }

                if (target.Cost != card.Cost)
                {
                    Console.WriteLine($"{card.Header} wrong {nameof(card.Cost)}.");
                }
            }
        }
    }

    public static void AnalyseCards()
    {
        var text = File.ReadAllText(StringConstants.CardsFullPath);
        var cards = JsonConvert.DeserializeObject<Card[]>(text)?.ToHashSet() ?? throw new Exception("Saved cards not parsed.");
        Console.WriteLine($"Saved cards loaded. Count: {cards.Count}.");

        var cardEffects = new List<CardEffect>();
        foreach (var card in cards)
        {
            var cardEffect = new CardEffect { Card = card };
            cardEffect.ResourceEffects = GetResourceEffects(card.Description);
            cardEffect.DamageEffects = GetDamageEffects(card.Description);
            cardEffects.Add(cardEffect);
        }

        cardEffects = cardEffects.OrderBy(x => x.Card.CardType).ThenBy(x => x.Card.Cost).ThenBy(x => x.Card.Header).ToList();


        var settings = new JsonSerializerSettings
        {
            Converters = { new StringEnumConverter() }
        };
        var json = JsonConvert.SerializeObject(cardEffects, Formatting.Indented, settings);
        File.WriteAllText(StringConstants.GeneratedCardEffectsFullPath, json);
        Console.WriteLine($"Card effects overwritten.");
    }

    private static List<ResourceEffect> GetResourceEffects(string description)
    {
        var resourceEffects = new List<ResourceEffect>();

        var resourceTypes = new ResourceType[] 
        {
            ResourceType.Ore,
            ResourceType.Mana,
            ResourceType.Stacks,
            ResourceType.Mines,
            ResourceType.Monasteries,
            ResourceType.Barracks,
            ResourceType.Tower,
            ResourceType.Wall
        };

        var sides = new Side[]
        {
            Side.Player,
            Side.Enemy
        };

        foreach (var resourceType in resourceTypes)
        {
            foreach (var resourceTypeDescription in resourceType.GetDescriptions())
            {
                foreach (var side in sides)
                {
                    foreach (var sideDescription in side.GetDescriptions(resourceType))
                    {
                        // FAMILIAR still should be manually adjusted.
                        var descriptionParts = description.Split(',');
                        foreach (var descriptionPart in descriptionParts)
                        {
                            var regexTemplate = resourceType.GetRegexTemplate();
                            var regex = string.Format(regexTemplate, resourceTypeDescription, sideDescription);

                            var match = Regex.Match(descriptionPart, regex);
                            if (match.Success)
                            {
                                foreach (var group in match.Groups.Values)
                                {
                                    if (int.TryParse(group.Value, out int value))
                                    {
                                        var resourceEffect = new ResourceEffect(resourceType, value, side);
                                        if (!resourceEffects.Any(x =>
                                            x.ResourceType == resourceEffect.ResourceType &&
                                            x.Value == resourceEffect.Value &&
                                            x.Side == resourceEffect.Side))
                                        {
                                            resourceEffects.Add(resourceEffect);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } 
        }

        return resourceEffects;
        
    }

    private static List<DamageEffect> GetDamageEffects(string description)
    {
        var damageEffects = new List<DamageEffect>();

        var damageTypes = new DamageType[]
        {
            DamageType.Tower,
            DamageType.Pure
        };

        var sides = new Side[]
        {
            Side.Player,
            Side.Enemy
        };

        foreach (var damageType in damageTypes)
        {
            foreach (var damageTypeDescription in damageType.GetDescriptions())
            {
                foreach (var side in sides)
                {
                    foreach (var sideDescription in side.GetDescriptions(damageType))
                    {
                        var descriptionParts = description.Split(',');
                        foreach (var descriptionPart in descriptionParts)
                        {
                            var regexTemplate = damageType.GetRegexTemplate();
                            var regex = string.Format(regexTemplate, damageTypeDescription, sideDescription);

                            var match = Regex.Match(descriptionPart, regex);
                            if (match.Success)
                            {
                                foreach (var group in match.Groups.Values)
                                {
                                    if (int.TryParse(group.Value, out int value))
                                    {
                                        var damageEffect = new DamageEffect(damageType, value, side);
                                        if (!damageEffects.Any(x =>
                                            x.DamageType == damageEffect.DamageType &&
                                            x.Value == damageEffect.Value &&
                                            x.Side == damageEffect.Side))
                                        {
                                            damageEffects.Add(damageEffect);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return damageEffects;
    }
}


