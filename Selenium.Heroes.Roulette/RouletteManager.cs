namespace Selenium.Heroes.Roulette;

public enum BetType
{
    Even,
    Odd
}

public static class RouletteManager
{
    private const int MinBet = 4000;

    private const int MaxBet = 4000;

    public static int Bet { get; set; } = MinBet;

    private static int CurrentLost { get; set; }

    private static Markers Markers { get; set; } = Markers.None;

    public static BetType SelectBetType()
    {
        return Bet == MaxBet ? BetType.Odd : BetType.Even;
    }

    public static void Mark(Markers marker)
    {
        Markers = Markers | marker;
    }

    public static void ResetMarkers()
    {
        Markers = Markers.None;
    }

    public static bool IsStarted()
    {
        return HasMarker(Markers.Started);
    }

    public static bool IsFinished()
    {
        return HasMarker(Markers.Finished);
    }

    public static bool HasMarker(Markers marker)
    {
        return Markers.HasFlag(marker);
    }

    public static void UpdateBet(bool success)
    {
        //if (success)
        //{
        //    CurrentLost = 0;
        //    Bet = MinBet;
        //    return;
        //}

        //CurrentLost = CurrentLost + Bet;
        //Console.WriteLine($"Lost : {CurrentLost}.");

        //Bet = GetBetPossibleWin() <= GetFurtherLost() ? Bet * 2 : Bet;
        //Console.WriteLine($"Bet : {Bet}.");
    }

    public static int GetBetPossibleWin() => Bet * 18;

    public static int GetFurtherLost() => CurrentLost + Bet;
}


[Flags]
public enum Markers
{
    None = 1,
    Started = 2,
    IsZeroFifelineBet = 4,
    IsSevenSixlineBet = 8,
    IsSecondDozenBet = 16,
    IsThirdDozenBet = 32,
    Black = 64,
    Even = 128,
    Odd = 256,
    Split = 512,
    Number = 1024,
    Finished = 2048,
}

