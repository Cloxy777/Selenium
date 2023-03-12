using Selenium.Heroes.Common;
using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;


// TODO:
// 1. Test correct weights. In plays =)
// 2. Understand when we play when we discard
// 3. Understand when we play weak and need to discard
// 4. Understand combinations


// Come up with some strategies to calculate card`s weight if another card played and operate by combinations inside
public class DecisionMaker
{
    public DecisionMaker(Player player, Player enemy, List<ICardDescriptor> cardDescriptors, Deck deck)
    {
        PlayerManager = new PlayerManager(player);
        EnemyManager = new PlayerManager(enemy);
        CardDescriptors = new List<ICardDescriptor>(cardDescriptors);
        Deck = new Deck(deck);
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public Deck Deck { get; }

    public Turn CreateTurn()
    {
        // ENEMY FIRST
        var enemyCards = BestEnemyDrawCards(PlayerManager, EnemyManager, Deck);
        var enemyBoard = new Board(EnemyManager, PlayerManager, enemyCards, Deck);
        var enemyAnalysis = new RecursiveAnalysis(enemyBoard);
        enemyAnalysis.Build();

        var enemyLeaves = new List<RecursiveAnalysis>();
        enemyAnalysis.Extract(ref enemyLeaves);
        enemyLeaves = enemyLeaves.OrderByDescending(x => x.SumRounds).ToList();

        var enemyWinnerTurn = enemyAnalysis.WinnerAnalysis;

        var enemyEffectiveAnalysis = enemyWinnerTurn ?? enemyLeaves.First();

        // PLAYER SECOND
        var playerBoard = new Board(PlayerManager, EnemyManager, CardDescriptors, Deck);
        var enemyTurns = enemyEffectiveAnalysis.Rounds.Select(x => x.PlayerTurn).ToList();
        var playerAnalysis = new RecursiveAnalysis(playerBoard) { EnemyTurns = enemyTurns };
        playerAnalysis.Build();

        var playerLeaves = new List<RecursiveAnalysis>();
        playerAnalysis.Extract(ref playerLeaves);
        playerLeaves = playerLeaves.OrderByDescending(x => x.SumRounds).ToList();

        //var check = leaves.Where(x => x.Rounds.All(y => y.PlayerTurn.Moves.First().ActionType == ActionType.Play)).ToList();

        var playerWinnerTurn = playerAnalysis.RecursiveAnalyses.FirstOrDefault(x => x.Board.PlayerManager.IsWinner || x.Board.EnemyManager.IsDestroed);

        var playerEffectiveAnalysis = playerWinnerTurn ?? playerLeaves.First();

        var turn = playerEffectiveAnalysis.Rounds.OrderBy(x => x.Order).Select(x => x.PlayerTurn).First();

        return turn;
    }

    public static List<ICardDescriptor> BestEnemyDrawCards(PlayerManager playerManager, PlayerManager enemyManager, Deck deck)
    {
        var enabledCards = deck.LeftCards.Where(x => x.IsEnabled(enemyManager)).ToList();

        CardEffect GetActualEffect(ICardDescriptor cardDescriptor)
        {
            return cardDescriptor.GetActualCardEffect(playerManager, enemyManager, new List<ICardDescriptor>(), new DRAGONS_HEART_CardDescriptor());
        }

        var cards = new List<ICardDescriptor>();
        if (enabledCards.Any())
        {
            var resourceEffectCards = enabledCards.Where(x => GetActualEffect(x).ResourceEffects.Any()).ToList();
            if (resourceEffectCards.Any())
            {
                var resourceTypes = new[]
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

                foreach (var resourceType in resourceTypes)
                {
                    var resourceCard = resourceEffectCards
                        .MaxBy(x => GetActualEffect(x).ResourceEffects
                            .Where(y => y.ResourceType == resourceType)
                            .DefaultIfEmpty()
                            .Max(y => y?.Value ?? 0));


                    if (resourceCard != null && cards.All(card => !card.Equals(resourceCard)))
                    {
                        cards.Add(resourceCard);
                    }
                }

                foreach (var resourceType in resourceTypes)
                {
                    var resourceCard = resourceEffectCards
                        .MinBy(x => GetActualEffect(x).ResourceEffects
                            .Where(y => y.ResourceType == resourceType)
                            .DefaultIfEmpty()
                            .Min(y => y?.Value ?? 0));


                    if (resourceCard != null && cards.All(card => !card.Equals(resourceCard)))
                    {
                        cards.Add(resourceCard);
                    }
                }
            }

            var damageEffectCards = enabledCards.Where(x => GetActualEffect(x).DamageEffects.Any()).ToList();
            if (damageEffectCards.Any())
            {
                var damageTypes = new[]
                {
                    DamageType.Pure,
                    DamageType.Tower,
                };

                foreach (var damageType in damageTypes)
                {
                    var damageCard = damageEffectCards
                        .MaxBy(x => GetActualEffect(x).DamageEffects
                            .Where(y => y.DamageType == damageType)
                            .DefaultIfEmpty()
                            .Max(y => y?.Value ?? 0));


                    if (damageCard != null && cards.All(card => !card.Equals(damageCard)))
                    {
                        cards.Add(damageCard);
                    }
                }

                foreach (var damageType in damageTypes)
                {
                    var damageCard = damageEffectCards
                      .MinBy(x => GetActualEffect(x).DamageEffects
                          .Where(y => y.DamageType == damageType)
                          .DefaultIfEmpty()
                          .Min(y => y?.Value ?? 0));


                    if (damageCard != null && cards.All(card => !card.Equals(damageCard)))
                    {
                        cards.Add(damageCard);
                    }
                }
            }
        }

        if (!cards.Any())
        {
            cards = enabledCards.Take(10).ToList();
        }

        return cards;
    }
}