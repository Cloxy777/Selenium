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

    public CardWeightCalculator Calculator { get; }

    public IEnumerable<CardWeight> CardWeights => Calculator.CardWeights;

    public decimal PlayerPower => PlayerManager.GetPower();

    public decimal EnemyPower => EnemyManager.GetPower();

    public decimal GetDisabledCardsPower()
    {
        var future = PlayerManager.Wait().Wait().Wait();
        var disabledCardsPower = CardWeights
            .Where(x => !x.CardDescriptor.IsEnabled(future))
            .Sum(x => x.CardDescriptor.ResourceLackNumber(future));

        return disabledCardsPower;
    }

    public Board Play(ICardDescriptor cardDescriptor, bool isProduce = true)
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

        playerManager = playerManager.ApplyCosts(cardDescriptor);

        if (isProduce)
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
            .Where(x => !x.Equals(cardDescriptor))
            .ToList();

        return new Board(playerManager, enemyManager, cardDescriptors);
    }

    public Board Discard(ICardDescriptor cardDescriptor, bool isProduce = true)
    {
        var playerManager = new PlayerManager(PlayerManager.Player);
        var enemyManager = new PlayerManager(EnemyManager.Player);

        if (isProduce)
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
            .Where(x => !x.Equals(cardDescriptor))
            .ToList();

        return new Board(playerManager, enemyManager, cardDescriptors);
    }

    public Board Make(Turn turn)
    {
        var board = new Board(PlayerManager, EnemyManager, CardDescriptors);
        foreach (var move in turn.Moves)
        {
            if (move.ActionType == ActionType.Play)
            {
                board = board.Play(move.CardDescriptor, move.IsProduce);
            }

            if (move.ActionType == ActionType.Discard)
            {
                board = board.Discard(move.CardDescriptor, move.IsProduce);
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

        var board = Play(cardDescriptor, false);

        foreach (var cardDescriptorItem in CardDescriptors.Where(x => !x.Equals(cardDescriptor)))
        {
            var discardedBoard = board.Discard(cardDescriptor, false);

            var turn = new Turn
            {
                Moves = new List<Move>
                {
                    new Move
                    {
                        ActionType = ActionType.Play,
                        CardDescriptor = cardDescriptor,
                        IsProduce = false
                    },
                    new Move
                    {
                        ActionType = ActionType.Discard,
                        CardDescriptor = cardDescriptorItem,
                        IsProduce = false
                    },
                }
            };

            turnes.AddRange(discardedBoard.GetPossibleDrawDiscardAndPlayAgainTurnes(turn));
        }


        return turnes;
    }

    private IEnumerable<Turn> GetPossibleDrawDiscardAndPlayAgainTurnes(Turn turn)
    {
        var turnes = new List<Turn>();

        foreach (var cardDescriptor in CardDescriptors)
        {
            var extendedTurn = new Turn(turn);
            extendedTurn.Moves.Add(new Move
            {
                ActionType = ActionType.Discard,
                CardDescriptor = cardDescriptor,
                IsProduce = true
            });
            turnes.Add(extendedTurn);

            if (cardDescriptor.IsEnabled(PlayerManager))
            {
                extendedTurn = new Turn(turn);
                extendedTurn.Moves.Add(new Move
                {
                    ActionType = ActionType.Play,
                    CardDescriptor = cardDescriptor,
                    IsProduce = true
                });
                turnes.Add(extendedTurn);
            }
        }

        return turnes;
    }

    public IEnumerable<Turn> GetPossiblePlayAgainTurnes(ICardDescriptor cardDescriptor)
    {
        var turnes = new List<Turn>();

        var board = Play(cardDescriptor, false);

        foreach (var cardDescriptorItem in CardDescriptors.Where(x => !x.Equals(cardDescriptor)))
        {
            if (cardDescriptorItem.IsEnabled(board.PlayerManager))
            {
                turnes.Add(new Turn
                {
                    Moves = new List<Move>
                    {
                        new Move
                        {
                            ActionType = ActionType.Play,
                            CardDescriptor = cardDescriptor,
                            IsProduce = false
                        },
                        new Move
                        {
                            ActionType = ActionType.Play,
                            CardDescriptor = cardDescriptorItem,
                            IsProduce = true
                        },
                    }
                });
            }

            turnes.Add(new Turn
            {
                Moves = new List<Move>
                {
                    new Move
                    {
                        ActionType = ActionType.Play,
                        CardDescriptor = cardDescriptor,
                        IsProduce = false
                    },
                    new Move
                    {
                        ActionType = ActionType.Discard,
                        CardDescriptor = cardDescriptorItem,
                        IsProduce = true
                    },
                }
            });
        }

        return turnes;
    }

    
}