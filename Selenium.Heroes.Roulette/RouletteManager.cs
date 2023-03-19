namespace Selenium.Heroes.Roulette;

public static class RouletteManager
{
    private const int MinBet = 100;

    public static int Bet { get; set; } = MinBet;

    public static bool IsWinCheck { get; set; } = true;

    public static bool IsMakeBets { get; set; } = false;

    private static bool isMakeZeroBet = true;
    public static bool IsMakeZeroBet()
    {
        isMakeZeroBet = true;
        return isMakeZeroBet;
    }

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

    public static bool AnyBets()
    {
        return IsZeroFifelineBet ||
            IsSevenSixlineBet ||
            IsSecondDozenBet ||
            IsThirdDozenBet;
    }

    internal static void UpdateBet(bool success)
    {
        Bet = success ? MinBet : Bet * 2;
        if (Bet > MinBet * 3) Bet = MinBet * 3;
        if (Bet == 0) Bet = MinBet;
    }
}
