﻿namespace Selenium.Heroes.Roulette;

// TODO: check if bet made on UI.

public class Startup
{
    public static HeroesRouletteEngine Engine { get; set; } = new HeroesRouletteEngine();

    public static void Run() 
    {
        Engine.Authenticate();
        Engine.NavigateToMain();
   
        while (true)
        {
            if (IsFirstMinute() && RouletteManager.IsStarted())
            {
                var winningNumber = Engine.GetLastWinningNumber();
                var isWin = IsEvenWin(winningNumber);
                RouletteManager.UpdateBet(isWin);
                RouletteManager.ResetMarkers();
            }

            if (IsFouthMinute())
            {           
                Engine.MakeBets();
            }

            WaitUntilNextMinute();           
        }     
    }

    private static DateTime NextRun { get; set; } = DateTime.Now;

    private static bool IsNextRun => DateTime.Now > NextRun;

    public static int Left => 5 - DateTime.Now.Minute % 5;

    public static void InternalRun()
    {       
        var isInTimeRange = IsFouthMinute() || IsThirdMinute() || IsSecondMinute() || IsFirstMinute();
        
        if (isInTimeRange && IsNextRun && RouletteManager.IsFinished())
        {
            var winningNumber = Engine.GetLastWinningNumber();
            var isWin = RouletteManager.SelectBetType() == BetType.Even ? IsEvenWin(winningNumber) : IsOddWin(winningNumber);
            RouletteManager.UpdateBet(isWin);
            RouletteManager.ResetMarkers();
            Console.WriteLine($"Roulette update bet. Bet={RouletteManager.Bet}.");
        }

        if ((isInTimeRange && IsNextRun) || (RouletteManager.IsStarted() && !RouletteManager.IsFinished()))
        {
            NextRun = DateTime.Now.AddMinutes(Left);
            Console.WriteLine($"Roulette started. NextRun={NextRun}.");

            Engine.MakeBets();
        }
    }

    private static bool IsFirstMinute() => DateTime.Now.Minute % 5 == 1;

    private static bool IsSecondMinute() => DateTime.Now.Minute % 5 == 2;

    private static bool IsThirdMinute() => DateTime.Now.Minute % 5 == 3;

    private static bool IsFouthMinute() => DateTime.Now.Minute % 5 == 4 && DateTime.Now.Second < 15;

    //private static bool IsWin(string numberText)
    //{
    //    var winningNumbers = new[]
    //    {
    //        "00",
    //        "0",
    //        "1",    "2",    "3",    "No",   "No",   "No",
    //        "7",    "8",    "9",    "10",   "11",   "12",
    //        "13",   "14",   "15",   "16",   "17",   "18",
    //        "19",   "20",   "21",   "22",   "23",   "24",
    //        "25",   "26",   "27",   "28",   "29",   "30",
    //        "31",   "32",   "33",   "34",   "35",   "36",
    //    };

    //    var success = winningNumbers.Any(x => x.Equals(numberText, StringComparison.OrdinalIgnoreCase));
    //    Console.WriteLine($"Win: {success}.");

    //    return success;
    //}

    private static bool IsOddWin(string numberText)
    {
        var winningNumbers = new[]
        {
            "",
            "",
            "1",    "",    "3",   "",   "No",   "",
            "7",    "",    "9",   "",   "11",   "",
            "13",   "",   "15",   "",   "17",   "",
            "19",   "",   "21",   "",   "23",   "",
            "25",   "",   "27",   "",   "29",   "",
            "31",   "",   "33",   "",   "35",   "",
        };

        var success = winningNumbers.Any(x => x.Equals(numberText, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine($"Win: {success}.");

        return success;
    }

    private static bool IsEvenWin(string numberText)
    {
        var winningNumbers = new[]
        {
            "",
            "",
            "",     "2",    "",     "4",    "",     "6",
            "",     "8",    "",     "10",   "",     "12",
            "",     "14",   "",     "16",   "",     "18",
            "",     "20",   "",     "22",   "",     "24",
            "",     "26",   "",     "28",   "",     "30",
            "",     "32",   "",     "34",   "",     "36",
        };

        var success = winningNumbers.Any(x => x.Equals(numberText, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine($"Win: {success}. {DateTime.Now}");

        return success;
    }

    private static void WaitUntilNextMinute()
    {
        var now = DateTime.Now;
        var currentMinuteDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 1);
        var nextMinuteDate = currentMinuteDate.AddMinutes(1);

        var diff = nextMinuteDate - now;

        Console.WriteLine($"Wait {(int)diff.TotalMilliseconds / 1000} seconds...");
        Thread.Sleep((int)diff.TotalMilliseconds);
    }
}
