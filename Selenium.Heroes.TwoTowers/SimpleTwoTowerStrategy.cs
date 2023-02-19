using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.TwoTowers;

public interface ITwoTowerStrategy
{

}

public class SimpleTwoTowerStrategy : ITwoTowerStrategy
{
    public SimpleTwoTowerStrategy(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors)
    {
        PlayerManager = playerManager;
        EnemyManager = enemyManager;
        CardDescriptors = cardDescriptors;
    }

    public PlayerManager PlayerManager { get; }

    public PlayerManager EnemyManager { get; }

    public List<ICardDescriptor> CardDescriptors { get; }

    public CardWeight GetMostEffectiveCardDescriptor()
    {
        var calculator = new CardWeightCalculator(PlayerManager, EnemyManager, CardDescriptors);

        var cardWeights = calculator.CardWeights;

        return cardWeights.Where(x => !x.CardDescriptor.IsEnabled(PlayerManager)).MaxBy(x => x.Weight)!;
    }
}