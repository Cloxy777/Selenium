namespace Selenium.Heroes.Roulette;

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
                var isWin = IsSplitWin(winningNumber);
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
            var isWin = IsSplitWin(winningNumber);
            RouletteManager.UpdateBet(isWin);
            RouletteManager.ResetMarkers();
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

    private static bool IsSplitWin(string numberText)
    {
        var winningNumbers = new[]
        {
            "00",
            "0"
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
