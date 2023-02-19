using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;
using System.Data.Common;
using System.Linq;

namespace Selenium.Heroes.TwoTowers;


// TODO:
// 1. Test correct weights. In plays =)
// 2. Understand when we play when we discard
// 3. Understand when we play weak and need to discard
// 4. Understand combinations


// Come up with some strategies to calculate card`s weight if another card played and operate by combinations inside
public class DecisionMaker
{
    public DecisionMaker(Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
	{
        PlayerManager = new PlayerManager(player);
        EnemyManager = new PlayerManager(enemy);
		CardDescriptors = cardDescriptors;
	}

	public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public Decision MakeDecision()
    {
        var board = new Board(PlayerManager, EnemyManager, CardDescriptors);
        var analysis = new RecursiveAnalysis(board);
        RecursiveAnalysis.MaxDeepLevel = Math.Max(3, board.EnabledCardDescriptors.Count);
        analysis.Build();

        var leaves = new List<RecursiveAnalysis>();
        analysis.Extract(ref leaves);

        var effectiveAnalysis = leaves.MaxBy(x => x.Rounds.Sum(x => x.Rating));

        var ordered = leaves.OrderByDescending(x => x.Rounds.Sum(x => x.Rating)).ToList();

        var turn = effectiveAnalysis!.Rounds.OrderBy(x => x.Order).Select(x => x.Turn).FirstOrDefault();
        
        var move = turn!.Moves.FirstOrDefault();

        return new Decision { ActionType = move.ActionType, CardDescriptor = move.CardDescriptor };
    }
}

public class Decision
{
    public ActionType ActionType { get; set; }

    public ICardDescriptor CardDescriptor { get; set; } = default!;
}

public enum ActionType
{
    Play,
    Discard
}