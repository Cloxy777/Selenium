namespace Selenium.Heroes.TwoTowers;

public class Round
{
    public Turn Turn { get; set; } = default!;

    public int Order { get; set; }

    public decimal Rating { get; set; }
}

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

        var disabledCardsPower = Board.GetMaxDisabledCardDebuff();

        var combinationBonus = 1 + (Rounds.OrderBy(x => x.Order)
            ?.LastOrDefault()
            ?.Turn
            ?.Moves?.Count ?? 0) / 10m;

        var positivePart = playerPower * playerCoefficient * combinationBonus;
        var negativePart = enemyPower * enemyCoefficient + disabledCardsPower;

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