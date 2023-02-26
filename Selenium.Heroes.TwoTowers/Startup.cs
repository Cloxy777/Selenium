using Selenium.Heroes.Common;

namespace Selenium.Heroes.TwoTowers;


// TODO: finish and start new game
public class Startup
{
    public static void Run() 
    {
        var isLocal = false;

        var engine = new HeroesTwoTowersEngine();
        engine.Authenticate();
        engine.NavigateToMain();

        //engine.NavigateToLocal();
        //isLocal = true;

        int seconds = 3;

        var deck = new Deck();
        while (true)
        {
            Console.ReadLine();

            var isCardGamePage = engine.IsCardGamePage();

            if (!isCardGamePage)
            {
                if (!engine.IsRegistered())
                {
                    engine.RegisterChallenge();

                    Console.WriteLine("Game registered.");

                    Thread.Sleep(seconds * 1000);
                    continue;
                }

                if (engine.IsPlayerFound())
                {
                    engine.SubmitPlayer();

                    Console.WriteLine("Player found.");

                    Thread.Sleep(seconds * 1000);
                    continue;
                }

                Console.WriteLine("Not in the card game.");
                Thread.Sleep(seconds * 1000);
                continue;
            }

            if (!engine.IsOurTurn())
            {
                Console.WriteLine("Not our turn.");
                Thread.Sleep(seconds * 1000);
                continue;
            }

            var player = engine.GetPlayerInfo();
            var enemy = engine.GetEnemyInfo();
            var cardDescriptors = engine.GetCardDescriptors();

            var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
            
            var turn = decisionMaker.CreateTurn();

            foreach ( var move in turn.Moves ) 
            {
                deck.Draw(move.CardDescriptor);

                if (move.ActionType == ActionType.Play)
                {
                    var card = move.CardDescriptor.BaseCardEffect.Card;

                    Console.WriteLine($"Play {card.Header}.");
                    engine.Play(card);
                    Thread.Sleep(seconds * 1000);
                }
                else if (move.ActionType == ActionType.Discard)
                {
                    var card = move.CardDescriptor.BaseCardEffect.Card;

                    Console.WriteLine($"Discard {card.Header}.");
                    engine.Discard(card);
                    Thread.Sleep(seconds * 1000);
                }
            }

            Thread.Sleep(seconds * 1000);
        }     
    }
}
