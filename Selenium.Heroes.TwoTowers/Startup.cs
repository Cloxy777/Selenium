using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Worker;

using WorkerStartup = Selenium.Heroes.Worker.Startup;
using RouletteStartup = Selenium.Heroes.Roulette.Startup;

namespace Selenium.Heroes.TwoTowers;

// TODO: finish and start new game
// Select lowest price
// Watch video and adapt
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

        var seconds = 3;

        var deck = new Deck();
        while (true)
        {
            RouletteStartup.InternalRun();

            if (engine.IsWorkAllowed())
            {
                Console.WriteLine("Work is allowed.");
                Console.WriteLine("Run worker..");
                WorkerStartup.InternalRun();

                Console.WriteLine("Continue card game..");
                Thread.Sleep(seconds * 1000);
                continue;

            }

            //if (engine.IsRoulettePage())
            //{
            //    engine.NavigateToMain();
            //}

            //var isCardGamePage = engine.IsCardGamePage();

            //if (!isCardGamePage)
            //{
            //    TurnCounter.Number = 1;
            //    Console.WriteLine($"Turn number = {TurnCounter.Number}.");

            //    if (!engine.IsRegistered())
            //    {
            //        engine.RegisterChallenge();

            //        Console.WriteLine("Game registered.");

            //        Thread.Sleep(seconds * 1000);
            //        continue;
            //    }

            //    if (engine.IsPlayerFound())
            //    {
            //        engine.SubmitPlayer();

            //        Console.WriteLine("Player found.");

            //        Thread.Sleep(seconds * 1000);
            //        continue;
            //    }

            //    Console.WriteLine("Not in the card game.");
            //    Thread.Sleep(seconds * 1000);
            //    continue;
            //}

            //// Provide some time window for worker.
            //if (engine.IsGameFinished())
            //{
            //    deck = new Deck();
            //    engine.Continue();

            //    Console.WriteLine("Run worker..");
            //    WorkerStartup.InternalRun();

            //    Console.WriteLine("Continue card game..");
            //    Thread.Sleep(seconds * 1000);
            //    continue;
            //}

            //if (!engine.IsOurTurn() || (!engine.TimeIsInRange(0, 38) && !engine.TimeIsInRange(42, 118)))
            //{
            //    Console.WriteLine("Not our turn.");
            //    Thread.Sleep(seconds * 1000);
            //    continue;
            //}

            //var player = engine.GetPlayerInfo();
            //var enemy = engine.GetEnemyInfo();
            //var cardDescriptors = engine.GetCardDescriptors();

            //deck = deck.Draw(cardDescriptors);
            //var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);

            //var turn = decisionMaker.CreateTurn();

            //var move = turn.Moves.First();
            //Play(move, deck, engine);
            //Thread.Sleep(seconds * 1000);

            //if (turn.Moves.Count == 3)
            //{
            //    move = turn.Moves[1];
            //    Play(move, deck, engine);
            //    Thread.Sleep(seconds * 1000);
            //}

            //if (turn.Moves.Count == 1)
            //{
            //    TurnCounter.Number++;
            //}

            Console.WriteLine($"Turn number = {TurnCounter.Number}.");
            Thread.Sleep(seconds * 1000);
        }     
    }

    public static void Play(Move move, Deck deck, HeroesTwoTowersEngine engine)
    {
        deck.Draw(move.CardDescriptor);
        var card = move.CardDescriptor.BaseCardEffect.Card;

        if (move.ActionType == ActionType.Play)
        {
            Console.WriteLine($"Play {card.Header}.");
            engine.Play(card);
        }
        else if (move.ActionType == ActionType.Discard)
        {
            Console.WriteLine($"Discard {card.Header}.");
            engine.Discard(card);
        }
    }
}
