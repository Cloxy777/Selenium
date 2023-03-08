namespace Selenium.Heroes.Common.Managers;

public static class TurnCounter
{
    public static int Number { get; set; } = 1;

    public static int Left => Math.Max(1, 20 - Number);
}
