namespace Selenium.Heroes.TwoTowers;

public class Round
{
    public Turn Turn { get; set; } = default!;

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

    public int CurrentDeepLevel { get; set; } = 0;

    public static int MaxDeepLevel { get; set; } = 4;

    public decimal GetCurrentPower()
    {
        var playerCoefficient = (1 + MaxDeepLevel / 10m - CurrentDeepLevel / 10m);
        var enemyCoefficient = (1 + CurrentDeepLevel / 10m);

        var playerPower = Board.PlayerPower;
        var enemyPower = Board.EnemyPower;

        var playerWinnerCoefficient = (Board.PlayerManager.IsWinner || Board.EnemyManager.IsDestroed) ? 10 : 1;
        var enemyWinnerCoefficient = (Board.EnemyManager.IsWinner || Board.PlayerManager.IsDestroed) ? 10 : 1;

        var disabledCardsPower = Board.GetMaxDisabledCardDebuff();

        var combinationBonus = 1 + (Rounds.OrderBy(x => x.Order)
            ?.LastOrDefault()
            ?.Turn
            ?.Moves?.Count(x => x.ActionType == ActionType.Play) ?? 0) / 10m;

        var positivePart = playerPower * playerCoefficient * combinationBonus * playerWinnerCoefficient;
        var negativePart = (enemyPower * enemyCoefficient + disabledCardsPower) * enemyWinnerCoefficient;

        return positivePart - negativePart;
    }

    public void Build()
    {
        if (CurrentDeepLevel >= MaxDeepLevel)
        {
            return;
        }

        var turnes = Board.GetPossibleTurnes();

        foreach (var turn in turnes)
        {
            var analysis = Make(turn);
            RecursiveAnalyses.Add(analysis);
        }

        foreach (var recursiveAnalysis in RecursiveAnalyses)
        {
            recursiveAnalysis.Build();
        }
    }

    public RecursiveAnalysis Make(Turn turn)
    {
        var board = new Board(Board);

        board = board.Make(turn);

        var analysis = new RecursiveAnalysis(board)
        {
            Rounds = new List<Round>(Rounds),
            CurrentDeepLevel = CurrentDeepLevel + 1
        };

        analysis.Rounds.Add(new Round
        {
            Turn = turn,
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