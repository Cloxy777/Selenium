using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;

public interface ITwoTowerStrategy
{

}

public class SimpleTwoTowerStrategy : ITwoTowerStrategy
{
    public SimpleTwoTowerStrategy(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        PlayerManager = playerManager;
        EnemyManager = enemyManager;
        CardDescriptors = cardDescriptors;
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public CardWeight GetMostEffectiveCardDescriptor()
    {
        var calculator = new CardWeightCalculator(PlayerManager, EnemyManager, CardDescriptors);

        var cardWeights = calculator.CardWeights;

        return cardWeights.Where(x => !x.CardDescriptor.IsEnabled(PlayerManager)).MaxBy(x => x.Weight)!;
    }
}

public class RecursiveAnalysis
{
    public RecursiveAnalysis(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        PlayerManager = playerManager;
        EnemyManager = enemyManager;
        CardDescriptors = cardDescriptors;
        Calculator = new CardWeightCalculator(PlayerManager, EnemyManager, CardDescriptors);
    }

    public List<RecursiveAnalysis> RecursiveAnalyses { get; set; } = new List<RecursiveAnalysis>();

    public List<Move> Moves { get; set; } = new List<Move>();

    public decimal Rating { get; set; } = 0;

    public int CurrentDeepLevel { get; set; } = 0;

    public int MaxDeepLevel => 6;

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public CardWeightCalculator Calculator { get; }

    public IEnumerable<CardWeight> CardWeights => Calculator.CardWeights;

    public decimal GetCurrentPower()
    {
        var playerPower = PlayerManager.GetPower();
        var enemyPower = EnemyManager.GetPower();

        var enabledCardsPower = CardWeights
            .Where(x => x.CardDescriptor.IsEnabled(PlayerManager))
            .Sum(x => x.Weight);

        var future = PlayerManager.Wait().Wait().Wait();
        var disabledCardsPower = CardWeights
            .Where(x => !x.CardDescriptor.IsEnabled(future))
            .Sum(x => x.CardDescriptor.ResourceLackNumber(future));

        return playerPower + enabledCardsPower - enemyPower - disabledCardsPower;
    }


    public void Build() 
    {
        if (CurrentDeepLevel >= MaxDeepLevel)
        {
            return;
        }

        var moves = GetPossibleMoves();

        foreach (var move in moves)
        {
            var analysis = Make(move);
            analysis.CurrentDeepLevel = CurrentDeepLevel + 1;

            var temp = new List<Move>(Moves);
            temp.Add(move);
            analysis.Moves = temp;
            analysis.Rating = Rating + GetCurrentPower();
            RecursiveAnalyses.Add(analysis);
        }

        foreach (var recursiveAnalysis in RecursiveAnalyses)
        {
            recursiveAnalysis.Build();
        }
    }

    public RecursiveAnalysis Make(Move move)
    {
        if (move.ActionType == ActionType.Play)
        {
            return Play(move.CardDescriptor);
        }

        if (move.ActionType == ActionType.Discard)
        {
            return Discard(move.CardDescriptor);
        }

        throw new NotSupportedException($"{nameof(Make)} not support {move.ActionType} action type.");
    }

    public List<Move> GetPossibleMoves()
    {
        var moves = new List<Move>();

        foreach (var cardDescriptor in CardDescriptors)
        {
            if (cardDescriptor.IsEnabled(PlayerManager))
            {
                moves.Add(new Move
                {
                    ActionType = ActionType.Play,
                    CardDescriptor = cardDescriptor,
                });
            }

            moves.Add(new Move
            {
                ActionType = ActionType.Discard,
                CardDescriptor = cardDescriptor,
            });
        }

        return moves;
    }

    private RecursiveAnalysis Play(ICardDescriptor cardDescriptor)
    {
        var actualEffect = cardDescriptor.GetActualCardEffect(PlayerManager, EnemyManager, CardDescriptors, cardDescriptor);

        var playerManager = new PlayerManager(PlayerManager);
        var enemyManager = new PlayerManager(EnemyManager);

        foreach (var resourceEffect in actualEffect.ResourceEffects)
        {
            if (resourceEffect.Side == Side.Player)
            {
                playerManager = playerManager.Apply(resourceEffect);
            }

            if (resourceEffect.Side == Side.Enemy)
            {
                enemyManager = enemyManager.Apply(resourceEffect);
            }
        }

        foreach (var damageEffect in actualEffect.DamageEffects)
        {
            if (damageEffect.Side == Side.Player)
            {
                playerManager = playerManager.Apply(damageEffect);
            }

            if (damageEffect.Side == Side.Enemy)
            {
                enemyManager = enemyManager.Apply(damageEffect);
            }
        }

        var cardDescriptors = CardDescriptors
            .Where(x => x.BaseCardEffect.Card.Header != cardDescriptor.BaseCardEffect.Card.Header)
            .ToList();

        if (cardDescriptor.BaseCardEffect.PlayType == PlayType.PlayAgain)
        {
            CurrentDeepLevel--;
        }

        return new RecursiveAnalysis(playerManager, enemyManager, cardDescriptors);
    }

    private RecursiveAnalysis Discard(ICardDescriptor cardDescriptor)
    {
        var playerManager = new PlayerManager(PlayerManager.Player)
            .Produce(ResourceType.Mines)
            .Produce(ResourceType.Monasteries)
            .Produce(ResourceType.Barracks);

        var enemyManager = new PlayerManager(EnemyManager.Player)
            .Produce(ResourceType.Mines)
            .Produce(ResourceType.Monasteries)
            .Produce(ResourceType.Barracks);

        var cardDescriptors = CardDescriptors
            .Where(x => x.BaseCardEffect.Card.Header != cardDescriptor.BaseCardEffect.Card.Header)
            .ToList();

        return new RecursiveAnalysis(playerManager, enemyManager, cardDescriptors);
    }

    public void Extract(ref List<RecursiveAnalysis> leaves)
    {
        foreach (var nested in RecursiveAnalyses)
        {
            if (!nested.RecursiveAnalyses.Any())
            {
                leaves.Add(nested);
                return;
            }

            nested.Extract(ref leaves);
        }
    }

    public CardWeight GetMostEffectiveCardDescriptor()
    {
        var calculator = new CardWeightCalculator(PlayerManager, EnemyManager, CardDescriptors);

        return CardWeights.Where(x => !x.CardDescriptor.IsEnabled(PlayerManager)).MaxBy(x => x.Weight)!;
    }
}

public class Move
{
    public ActionType ActionType { get; set; }

    public ICardDescriptor CardDescriptor { get; set; } = default!;
}