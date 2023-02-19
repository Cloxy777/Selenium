
namespace Selenium.Heroes.TwoTowers;

public class RecursiveAnalysis
{
    public RecursiveAnalysis(Board board)
    {
        Board = board;
    }
    public Board Board { get; }

    public List<RecursiveAnalysis> RecursiveAnalyses { get; set; } = new List<RecursiveAnalysis>();

    public List<Turn> Turnes { get; set; } = new List<Turn>();

    public List<decimal> Ratings { get; set; } = new List<decimal>();

    public int CurrentDeepLevel { get; set; } = 0;

    public int MaxDeepLevel => 4;

    public decimal GetCurrentPower()
    {
        var playerCoefficient = (1 + MaxDeepLevel / 10m - CurrentDeepLevel / 10m);
        var enemyCoefficient = (1 + CurrentDeepLevel / 10m);

        var playerPower = Board.PlayerPower * playerCoefficient;
        var enemyPower = Board.EnemyPower * enemyCoefficient;

        var disabledCardsPower = Board.GetDisabledCardsPower();

        return playerPower - enemyPower - disabledCardsPower;
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
            Turnes = new List<Turn>(Turnes)
            {
                turn
            },
            Ratings = new List<decimal>(Ratings)
            {
                GetCurrentPower()
            },
            CurrentDeepLevel = CurrentDeepLevel + 1
        };

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