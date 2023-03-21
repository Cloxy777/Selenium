namespace Selenium.Heroes.Roulette;

public static class RouletteManager
{
    private const int MinBet = 100;

    public static int Bet { get; set; } = MinBet;

    public static bool IsZeroFifelineBet { get; set; } = false;
    public static bool IsSevenSixlineBet { get; set; } = false;
    public static bool IsSecondDozenBet { get; set; } = false;
    public static bool IsThirdDozenBet { get; set; } = false;

    public static void ResetBetMarkers()
    {
        IsZeroFifelineBet = false;
        IsSevenSixlineBet = false;
        IsSecondDozenBet = false;
        IsThirdDozenBet = false;
    }

    public static bool IsAnyBetMarkers()
    {
        return IsZeroFifelineBet ||
            IsSevenSixlineBet ||
            IsSecondDozenBet ||
            IsThirdDozenBet;
    }

    public static bool IsAllBetMarkers()
    {
        return IsZeroFifelineBet &&
            IsSevenSixlineBet &&
            IsSecondDozenBet &&
            IsThirdDozenBet;

    }

    public static void UpdateBet(bool success)
    {
        Bet = success ? MinBet : Bet * 2;
        if (Bet > MinBet * 4) Bet = MinBet * 4;
        if (Bet == 0) Bet = MinBet;
    }
}
