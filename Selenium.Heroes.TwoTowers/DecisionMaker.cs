using Selenium.Heroes.Common;
using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;


// TODO:
// 1. Test correct weights. In plays =)
// 2. Understand when we play when we discard
// 3. Understand when we play weak and need to discard
// 4. Understand combinations


// Come up with some strategies to calculate card`s weight if another card played and operate by combinations inside
public class DecisionMaker
{
    public DecisionMaker(Player player, Player enemy, List<ICardDescriptor> cardDescriptors, Deck deck)
    {
        PlayerManager = new PlayerManager(player);
        EnemyManager = new PlayerManager(enemy);
        CardDescriptors = new List<ICardDescriptor>(cardDescriptors);
        Deck = new Deck(deck);
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public Deck Deck { get; }

    public Turn CreateTurn()
    {
        var board = new Board(PlayerManager, EnemyManager, CardDescriptors, Deck);
        var analysis = new RecursiveAnalysis(board);
        analysis.Build();

        var leaves = new List<RecursiveAnalysis>();
        analysis.Extract(ref leaves);
        leaves = leaves.OrderByDescending(x => x.Rounds.Sum(x => x.Rating)).ToList();

        var winnerTurn = analysis.RecursiveAnalyses.FirstOrDefault(x => x.Board.PlayerManager.IsWinner || x.Board.EnemyManager.IsDestroed);

        var effectiveAnalysis = winnerTurn ?? leaves.MaxBy(x => x.Rounds.Sum(x => x.Rating));

        var turn = effectiveAnalysis!.Rounds.OrderBy(x => x.Order).Select(x => x.PlayerTurn).FirstOrDefault();

        return turn!;
    }
}