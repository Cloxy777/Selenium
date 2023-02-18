using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
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
    private const decimal WEIGHT_THRESHOLD = 0.3m;

    public DecisionMaker(Player player, Player enemy, List<ICardDescriptor> cardDescriptors)
	{
        PlayerManager = new PlayerManager(player);
        EnemyManager = new PlayerManager(enemy);
		CardDescriptors = cardDescriptors;
	}

	public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    //public Decision MakeDecision()
    //{
    //    var canPlay = CardDescriptors.Any(x => x.IsEnabled(PlayerManager));
    //    var actionType = canPlay ? ActionType.Play : ActionType.Discard;

    //    if (canPlay)
    //    {
    //        var strategy = new SimpleTwoTowerStrategy(PlayerManager, EnemyManager, CardDescriptors);    
    //        var cardWeight = strategy.GetMostEffectiveCardDescriptor();

    //        if (cardWeight.Weight > WEIGHT_THRESHOLD)
    //        {
    //            var decision = new Decision
    //            {
    //                ActionType = ActionType.Play,
    //                CardDescriptor = cardWeight.CardDescriptor
    //            };
    //            return decision;
    //        }
    //    }

    //    return new Decision
    //    {
    //        ActionType = ActionType.Discard,
    //        CardDescriptor = GetCardDescriptorToDiscard(),
    //    };
    //}

    public Decision MakeDecision()
    {
        var analysis = new RecursiveAnalysis(PlayerManager, EnemyManager, CardDescriptors);
        analysis.Build();

        var leaves = new List<RecursiveAnalysis>();
        analysis.Extract(ref leaves);

        var effectiveAnalysis = leaves.MaxBy(x => x.Rating);

        var ordered = leaves.OrderByDescending(x => x.Rating).ToList();

        var move = effectiveAnalysis.Moves.First();

        return new Decision { ActionType = move.ActionType, CardDescriptor = move.CardDescriptor };
    }

    private ICardDescriptor GetCardDescriptorToDiscard()
    {
        var future = PlayerManager.Wait().Wait().Wait();

        return CardDescriptors
            .Where(x => !x.IsEnabled(future))
            .OrderByDescending(x => x.ResourceLackNumber(future))
            .First();
    }
}