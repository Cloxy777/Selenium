namespace Selenium.Heroes.Roulette;

public static class RouletteManager
{
    private const int MinBet = 145;

    public static int Bet { get; set; } = MinBet;


    private static Markers Markers { get; set; } = Markers.None;

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
        if (Bet > MinBet * 4) Bet = MinBet * 4;
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
    Finished = 64,
}

