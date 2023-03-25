namespace Selenium.Heroes.Roulette;

public enum BetType
{
    Even,
    Odd
}

public static class RouletteManager
{
    private const int MinBet = 100;

    private const int MaxBet = 4000;

    public static int Bet { get; set; } = MinBet;


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
        Bet = success ? MinBet : Bet * 2;
        if (Bet > MaxBet) Bet = MinBet;
        if (Bet < MinBet) Bet = MinBet;
    }
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
    Finished = 256,
}

