using Selenium.Heroes.Common;
using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;

public class Turn
{
    public Turn()
    {

    }

    public Turn(Turn turn)
    {
        Moves = new List<Move>(turn.Moves);
    }

    public List<Move> Moves { get; set; } = new List<Move>();
}

public class Move
{
    public ActionType ActionType { get; set; }

    public bool IsProduce { get; set; } = true;

    public ICardDescriptor CardDescriptor { get; set; } = default!;
}

public enum ActionType
{
    Play,
    Discard
}

public class Board
{
    public Board(Board board) : this (board.PlayerManager, board.EnemyManager, board.CardDescriptors)
    {  }

    public Board(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        PlayerManager = new PlayerManager(playerManager);
        EnemyManager = new PlayerManager(enemyManager);
        CardDescriptors = new List<ICardDescriptor>(cardDescriptors!);

        Calculator = new CardWeightCalculator(this);
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public List<ICardDescriptor> EnabledCardDescriptors 
        => CardDescriptors.Where(x => x.IsEnabled(PlayerManager)).ToList();

    public CardWeightCalculator Calculator { get; }

    public IEnumerable<CardWeight> CardWeights => Calculator.CardWeights;

    public decimal PlayerPower => PlayerManager.GetPower();

    public decimal EnemyPower => EnemyManager.GetPower();


    public decimal GetMaxDisabledCardDebuff()
    {
        var future = PlayerManager.Wait().Wait().Wait();
        var disabledCardsPower = CardDescriptors
            .Where(x => !x.IsEnabled(future))
            .DefaultIfEmpty()
            .Max(x => x?.ResourceLackNumber(future) ?? 0);

        return disabledCardsPower;
    }

    public Board Play(Move move)
    {
        var playerManager = new PlayerManager(PlayerManager);
        var enemyManager = new PlayerManager(EnemyManager);

        var actualEffect = move.CardDescriptor.GetActualCardEffect(playerManager, enemyManager, CardDescriptors, move.CardDescriptor);

        playerManager = playerManager.ApplyCosts(move.CardDescriptor);

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

        if (move.IsProduce)
        {
            playerManager = playerManager
               .Produce(ResourceType.Mines)
               .Produce(ResourceType.Monasteries)
               .Produce(ResourceType.Barracks);

            enemyManager = enemyManager
                .Produce(ResourceType.Mines)
                .Produce(ResourceType.Monasteries)
                .Produce(ResourceType.Barracks);
        }

        var cardDescriptors = CardDescriptors
            .Where(x => !x.Equals(move.CardDescriptor))
            .ToList();

        return new Board(playerManager, enemyManager, cardDescriptors);
    }

    public Board Discard(Move move)
    {
        var playerManager = new PlayerManager(PlayerManager);
        var enemyManager = new PlayerManager(EnemyManager);

        if (move.IsProduce)
        {
            playerManager = playerManager
                .Produce(ResourceType.Mines)
                .Produce(ResourceType.Monasteries)
                .Produce(ResourceType.Barracks);

            enemyManager = enemyManager
                .Produce(ResourceType.Mines)
                .Produce(ResourceType.Monasteries)
                .Produce(ResourceType.Barracks);
        }

        var cardDescriptors = CardDescriptors
            .Where(x => !x.Equals(move.CardDescriptor))
            .ToList();

        return new Board(playerManager, enemyManager, cardDescriptors);
    }

    public Board Make(Turn turn)
    {
        var board = new Board(this);
        foreach (var move in turn.Moves)
        {
            if (move.ActionType == ActionType.Play)
            {
                board = board.Play(move);
            }

            if (move.ActionType == ActionType.Discard)
            {
                board = board.Discard(move);
            }
        }

        return board;
    }

    public IEnumerable<Turn> GetPossibleTurnes()
    {
        var turnes = new List<Turn>();

        foreach (var cardDescriptor in CardDescriptors)
        {
            if (cardDescriptor.IsEnabled(PlayerManager))
            {
                if (cardDescriptor.BaseCardEffect.PlayType is PlayType.PlayAgain)
                {
                    turnes.AddRange(GetPossiblePlayAgainTurnes(cardDescriptor));
                }
                else if (cardDescriptor.BaseCardEffect.PlayType is PlayType.DrawDiscardAndPlayAgain)
                {
                    turnes.AddRange(GetPossibleDrawDiscardAndPlayAgainTurnes(cardDescriptor));
                }
                else
                {
                    turnes.Add(new Turn
                    {
                        Moves = new List<Move>
                        {
                            new Move
                            {
                                ActionType = ActionType.Play,
                                CardDescriptor = cardDescriptor,
                                IsProduce = true
                            }
                        }
                    });
                }
            }

            turnes.Add(new Turn
            {
                Moves = new List<Move>
                {
                    new Move
                    {
                        ActionType = ActionType.Discard,
                        CardDescriptor = cardDescriptor,
                        IsProduce = true
                    }
                }
            });
        }

        return turnes;
    }

    public IEnumerable<Turn> GetPossibleDrawDiscardAndPlayAgainTurnes(ICardDescriptor cardDescriptor)
    {
        var turnes = new List<Turn>();

        var move = new Move
        {
            ActionType = ActionType.Play,
            CardDescriptor = cardDescriptor,
            IsProduce = false
        };

        var turn = new Turn();
        turn.Moves.Add(move);

        var board = Play(move);

        foreach (var cardDescriptorItem in board.CardDescriptors)
        {
            var extendedTurn = new Turn(turn);
            move = new Move
            {
                ActionType = ActionType.Discard,
                CardDescriptor = cardDescriptorItem,
                IsProduce = false
            };
            extendedTurn.Moves.Add(move);

            var afterDiscard = board.Discard(move);
            turnes.AddRange(afterDiscard.GetPossiblePlayAgainTurnes(extendedTurn));
        }


        return turnes;
    }

    public IEnumerable<Turn> GetPossiblePlayAgainTurnes(ICardDescriptor cardDescriptor)
    {
        var move = new Move
        {
            ActionType = ActionType.Play,
            CardDescriptor = cardDescriptor,
            IsProduce = false
        };

        var turn = new Turn();
        turn.Moves.Add(move);

        var board = Play(move);
        return board.GetPossiblePlayAgainTurnes(turn);
    }

    private IEnumerable<Turn> GetPossiblePlayAgainTurnes(Turn turn)
    {
        var turnes = new List<Turn>();

        foreach (var cardDescriptor in CardDescriptors)
        {
            var extendedTurn = new Turn(turn);
            var move = new Move
            {
                ActionType = ActionType.Discard,
                CardDescriptor = cardDescriptor,
                IsProduce = true
            };
            extendedTurn.Moves.Add(move);
            turnes.Add(extendedTurn);

            if (cardDescriptor.IsEnabled(PlayerManager))
            {
                extendedTurn = new Turn(turn);
                move = new Move
                {
                    ActionType = ActionType.Play,
                    CardDescriptor = cardDescriptor,
                    IsProduce = true
                };
                extendedTurn.Moves.Add(move);
                turnes.Add(extendedTurn);
            }
        }

        return turnes;
    }
}