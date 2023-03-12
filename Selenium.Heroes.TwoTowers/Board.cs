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

    public Board Play(Move move, Side side = Side.Player)
    {
        var playerManager = side is Side.Player ? new PlayerManager(PlayerManager) : new PlayerManager(EnemyManager);
        var enemyManager = side is Side.Player ? new PlayerManager(EnemyManager) : new PlayerManager(PlayerManager);

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

        return side is Side.Player ? 
            new Board(playerManager, enemyManager, cardDescriptors, deck) :
            new Board(enemyManager, playerManager, cardDescriptors, deck);
    }

    public Board Discard(Move move, Side side = Side.Player)
    {
        var playerManager = side is Side.Player ? new PlayerManager(PlayerManager) : new PlayerManager(EnemyManager);
        var enemyManager = side is Side.Player ? new PlayerManager(EnemyManager) : new PlayerManager(PlayerManager);

        if (move.IsProduce)
        {
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

        return side is Side.Player ?
            new Board(playerManager, enemyManager, cardDescriptors, deck) :
            new Board(enemyManager, playerManager, cardDescriptors, deck);
    }

    public Board Make(Turn turn)
    {
        var board = Play(turn);
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

    // Play or Discard is must for enemy because Player gets resources here.
    public Board EnemyPlayOrDiscard(Turn enemyTurn)
    {
        var board = new Board(this);
        foreach (var move in enemyTurn.Moves)
        {
            if (move.ActionType == ActionType.Play && move.CardDescriptor.IsEnabled(EnemyManager))
            {
                board = board.Play(move, Side.Enemy);
                continue;
            }

            board = board.Discard(move, Side.Enemy);
        }

        return board;
    }

    public IEnumerable<Turn> GetPossibleTurnes(bool ignoreDiscard = false)
    {
        var turnes = new List<Turn>();

        foreach (var cardDescriptor in CardDescriptors.OrderByDescending(x => x.BaseCardEffect.Card.Cost))
        {
            if (cardDescriptor.IsEnabled(PlayerManager))
            {
                if (cardDescriptor.BaseCardEffect.PlayType is PlayType.PlayAgain)
                {
                    turnes.AddRange(GetPossiblePlayAgainTurnes(cardDescriptor, ignoreDiscard));
                }
                else if (cardDescriptor.BaseCardEffect.PlayType is PlayType.DrawDiscardAndPlayAgain)
                {
                    turnes.AddRange(GetPossibleDrawDiscardAndPlayAgainTurnes(cardDescriptor, ignoreDiscard));
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

            if (!ignoreDiscard)
            {
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
        }

        return turnes;
    }

    public IEnumerable<Turn> GetPossibleDrawDiscardAndPlayAgainTurnes(ICardDescriptor cardDescriptor, bool ignoreDiscard = false)
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
            turnes.AddRange(afterDiscard.GetPossiblePlayAgainTurnes(extendedTurn, ignoreDiscard));
        }


        return turnes;
    }

    public IEnumerable<Turn> GetPossiblePlayAgainTurnes(ICardDescriptor cardDescriptor, bool ignoreDiscard =  false)
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

        return board.GetPossiblePlayAgainTurnes(turn, ignoreDiscard);
    }

    private IEnumerable<Turn> GetPossiblePlayAgainTurnes(Turn turn, bool ignoreDiscard = false)
    {
        var drawCard = Deck.LeftCards.OrderBy(x => x.BaseCardEffect.Card.Cost).First();
        CardDescriptors.Add(drawCard);

        var turnes = new List<Turn>();

        foreach (var cardDescriptor in CardDescriptors.OrderByDescending(x => x.BaseCardEffect.Card.Cost))
        {
            if (!ignoreDiscard)
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

            }

            if (cardDescriptor.IsEnabled(PlayerManager))
            {
                var extendedTurn = new Turn(turn);
                var move = new Move
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