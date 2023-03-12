using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;

public class Round
{
    public Turn PlayerTurn { get; set; } = default!;

    public Turn EnemyTurn { get; internal set; } = default!;

    public int Order { get; set; }

    public decimal Rating { get; set; }
}

// TODO:
// Adjust power to FINISH the game if possible (Tower adjust !!!)
// Adjust to not discard with play again if possible
// Still doesn't attack tower
// Still plays SUBSOIL WATERS
// Still plays ELVEN SCOUTS bad (discards third card)
// COLLABORATION not played on 0 Ore
// For rest of the cards calculate the best combination for enemy 
// calculate our turns based on enmy possible turns

public class RecursiveAnalysis
{
    public RecursiveAnalysis(Board board)
    {
        Board = new Board(board);
    }
    public Board Board { get; }

    public List<RecursiveAnalysis> RecursiveAnalyses { get; set; } = new List<RecursiveAnalysis>();

    public List<Round> Rounds { get; set; } = new List<Round>();

    public decimal SumRounds => Rounds.Sum(x => x.Rating);

    public int CurrentDeepLevel { get; set; } = 0;

    public int MaxDeepLevel { get; set; } = 3;

    public List<Turn> EnemyTurns { get; set; } = new List<Turn>();

    public RecursiveAnalysis? WinnerAnalysis => RecursiveAnalyses.FirstOrDefault(x => x.Board.PlayerManager.IsWinner || x.Board.EnemyManager.IsDestroed);

    public decimal GetCurrentPower()
    {
        var earlierBetter = 1.1m - (0.1m / MaxDeepLevel * CurrentDeepLevel);

        var playerPower = Board.PlayerPower;
        var enemyPower = Board.EnemyPower;

        var playerWinnerCoefficient = (Board.PlayerManager.IsWinner || Board.EnemyManager.IsDestroed) ? 10 : 1;
        var enemyWinnerCoefficient = (Board.EnemyManager.IsWinner || Board.PlayerManager.IsDestroed) ? 10 : 1;

        var positivePart = playerPower * playerWinnerCoefficient * earlierBetter;
        var negativePart = enemyPower * enemyWinnerCoefficient;

        return positivePart - negativePart;
    }

    public void Build()
    {
        if (CurrentDeepLevel >= MaxDeepLevel)
        {
            return;
        }

        var playerTurnes = Board.GetPossiblePlayerTurnes();
        
        foreach (var playerTurn in playerTurnes)
        {
            var analysis = Make(playerTurn);
            RecursiveAnalyses.Add(analysis);
        }

        foreach (var recursiveAnalysis in RecursiveAnalyses)
        {
            recursiveAnalysis.Build();
        }
    }

    public RecursiveAnalysis Make(Turn playerTurn)
    {
        var board = new Board(Board);

        board = board.Make(playerTurn);

        var enemyTurn = EnemyTurns.FirstOrDefault();
        if (!board.PlayerManager.IsWinner && !board.EnemyManager.IsDestroed && enemyTurn != null)
        {
            board = board.Play(enemyTurn, Side.Enemy);
        }

        var enemyTurns = new List<Turn>(EnemyTurns);
        if (enemyTurns.Any())
        {
            enemyTurns.RemoveAt(0);
        }
        var analysis = new RecursiveAnalysis(board)
        {
            Rounds = new List<Round>(Rounds),
            CurrentDeepLevel = CurrentDeepLevel + 1,
            EnemyTurns = enemyTurns
        };

        analysis.Rounds.Add(new Round
        {
            PlayerTurn = playerTurn,
            EnemyTurn = enemyTurn,
            Rating = analysis.GetCurrentPower(),
            Order = analysis.CurrentDeepLevel
        });

        return analysis;
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
}