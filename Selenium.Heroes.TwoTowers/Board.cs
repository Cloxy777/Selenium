using Selenium.Heroes.Common;
using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Loaders;
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
    public Board(Board board) : this (board.PlayerManager, board.EnemyManager, board.CardDescriptors, board.Deck)
    {  }

    public Board(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, Deck deck)
    {
        PlayerManager = new PlayerManager(playerManager);
        EnemyManager = new PlayerManager(enemyManager);
        CardDescriptors = new List<ICardDescriptor>(cardDescriptors!);
        Deck = new Deck(deck);

        Calculator = new CardWeightCalculator(this);
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public Deck Deck { get; }

    public List<ICardDescriptor> EnabledCardDescriptors 
        => CardDescriptors.Where(x => x.IsEnabled(PlayerManager)).ToList();

    public CardWeightCalculator Calculator { get; }

    public IEnumerable<CardWeight> CardWeights => Calculator.CardWeights;

    public decimal PlayerPower => PlayerManager.GetPower(EnemyManager);

    public decimal EnemyPower => EnemyManager.GetPower(PlayerManager);


    public decimal GetMaxDisabledCardDebuff()
    {
        var future = PlayerManager.Wait().Wait().Wait();
        var disabledCardsPower = CardDescriptors
            .Where(x => !x.IsEnabled(future))
            .DefaultIfEmpty()
            .Sum(x => x?.ResourceLackNumber(future) ?? 0);

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

        var deck = new Deck(Deck);
        deck = deck.Draw(move.CardDescriptor);

        return new Board(playerManager, enemyManager, cardDescriptors, deck);
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

        var deck = new Deck(Deck);
        deck = deck.Draw(move.CardDescriptor);

        return new Board(playerManager, enemyManager, cardDescriptors, deck);
    }

    public Board Make(Turn playerTurn)
    {
        var board = Play(playerTurn);

        var enemyTurnes = board.GetPossibleEnemyTurnes();
        var enemyTurn = board.GetBestEnemyTurn(enemyTurnes);
        board = board.Play(enemyTurn);

        return board;
    }

    public Board Play(Turn turn)
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

    public IEnumerable<Turn> GetPossiblePlayerTurnes()
    {
        return GetPossibleTurnes(PlayerManager, CardDescriptors);
    }

    public IEnumerable<Turn> GetPossibleEnemyTurnes()
    {
        return GetPossibleTurnes(EnemyManager, Deck.LeftCards.ToList());
    }

    public IEnumerable<Turn> GetPossibleTurnes(PlayerManager playerManager, List<ICardDescriptor> cardDescriptors)
    {
        var turnes = new List<Turn>();

        foreach (var cardDescriptor in cardDescriptors)
        {
            if (cardDescriptor.IsEnabled(playerManager))
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

    public Turn GetBestEnemyTurn(IEnumerable<Turn> turnes)
    {
        var best = turnes.MaxBy(turn =>
        {
            var board = Play(turn);
            return board.EnemyPower - board.PlayerPower;
        });

        return best;
    }
}