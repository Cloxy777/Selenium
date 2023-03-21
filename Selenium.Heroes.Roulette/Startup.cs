namespace Selenium.Heroes.Roulette;

public class Startup
{
    public static HeroesRouletteEngine Engine { get; set; } = new HeroesRouletteEngine();

    public static void Run() 
    {
        Engine.Authenticate();
        Engine.NavigateToMain();
   
        while (true)
        {
            if (IsFirstMinute() && RouletteManager.IsAnyBetMarkers())
            {
                var winningNumber = Engine.GetLastWinningNumber();
                var isWin = IsWin(winningNumber);
                RouletteManager.UpdateBet(isWin);
                RouletteManager.ResetBetMarkers();
            }

            if (IsFouthMinute())
            {           
                Engine.MakeBets();
            }

            WaitUntilNextMinute();           
        }     
    }

    private static DateTime InternalTreshhold { get; set; } = DateTime.Now;

    public static void InternalRun()
    {
        //RouletteManager.Bet = 200;
        //var winningNumber1 = Engine.GetLastWinningNumber();
        //var isWin1 = IsWin(winningNumber1);
        //RouletteManager.UpdateBet(isWin1);

        var isAnyBetMarkersMissed = RouletteManager.IsAnyBetMarkers() && !RouletteManager.IsAllBetMarkers();
        var isInTime = IsFouthMinute() || IsThirdMinute() || IsSecondMinute() || IsFirstMinute();
        var ok = Engine.Started && !Engine.Finished || Engine.Finished && !Engine.Started;

        if (isInTime && (DateTime.Now > InternalTreshhold || isAnyBetMarkersMissed) || ok)
        {
            InternalTreshhold = DateTime.Now.AddMinutes(4);

            Console.WriteLine($"Roulette started. InternalTreshhold={InternalTreshhold}.");
            Console.WriteLine($"Roulette started. Initial bet={RouletteManager.Bet}.");

            if (!isAnyBetMarkersMissed)
            {
                var winningNumber = Engine.GetLastWinningNumber();
                var isWin = IsWin(winningNumber);
                RouletteManager.UpdateBet(isWin);
            }
            
            Console.WriteLine($"Roulette processing. Initial bet={RouletteManager.Bet}.");
            
            Engine.MakeBets();
            RouletteManager.ResetBetMarkers();
        }
    }

    private static bool IsFirstMinute() => DateTime.Now.Minute % 5 == 1;

    private static bool IsSecondMinute() => DateTime.Now.Minute % 5 == 2;

    private static bool IsThirdMinute() => DateTime.Now.Minute % 5 == 3;

    private static bool IsFouthMinute() => DateTime.Now.Minute % 5 == 4;

    private static bool IsWin(string numberText)
    {
        var winningNumbers = new[]
        {
            "00",
            "0",
            "1",    "2",    "3",    "No",   "No",   "No",
            "7",    "8",    "9",    "10",   "11",   "12",
            "13",   "14",   "15",   "16",   "17",   "18",
            "19",   "20",   "21",   "22",   "23",   "23",
            "25",   "26",   "27",   "28",   "29",   "30",
            "31",   "32",   "33",   "34",   "35",   "36",
        };

        var success = winningNumbers.Any(x => x.Equals(numberText, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine($"Win: {success}.");

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
