namespace Selenium.Heroes.TwoTowers;

public class Startup
{
    public static void Run() 
    {
        var isLocal = false;

        var engine = new HeroesTwoTowersEngine();
        engine.NavigateToMain();

        //engine.NavigateToLocal();
        //isLocal = true;

        int seconds = 5;
        while (true)
        {
            Console.ReadLine();
            var isCardGamePage = engine.IsCardGamePage();

            if (!isCardGamePage && !isLocal)
            {
                Console.WriteLine("Not in the card game.");
                seconds = 5;
                Thread.Sleep(seconds * 1000);
                continue;
            }

            var player = engine.GetPlayerInfo();
            var enemy = engine.GetEnemyInfo();
            var cardDescriptors = engine.GetCardDescriptors();

            var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors);
            
            var decision = decisionMaker.MakeDecision();

            if (decision.ActionType == ActionType.Play)
            {
                var card = decision.CardDescriptor.BaseCardEffect.Card;

                Console.WriteLine($"Play {card.Header}.");
                //engine.Play(card);          
            }
            else if (decision.ActionType == ActionType.Discard)
            {
                var card = decision.CardDescriptor.BaseCardEffect.Card;

                Console.WriteLine($"Discard {card.Header}.");
                //engine.Discard(card);
            }

            seconds = 2;
            Thread.Sleep(seconds * 1000);
        }     
    }
}
