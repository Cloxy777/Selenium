using OpenQA.Selenium;
using Selenium.Heroes.Common;

namespace Selenium.Heroes.Roulette;

public class HeroesRouletteEngine : HeroesEngineBase
{
    private const int SleepTime = 300;

    private const string SelectTileWarning = "Please choose a tile or crossing to bet on";
    private const string IncorrectBetWarning = "Incorrect bet";

    private const string ThirdDozenSelector = "//img[@title='3rd Dozen']";
    private const string SecondDozenSelector = "//img[@title='2nd Dozen']";
    private const string FirstDozenSelector = "//img[@title='1rd Dozen']";

    private const string BlackSelector = "//img[@title='BLACK']";

    private const string EvenSelector = "//img[@title='EVEN']";
    private const string OddSelector = "//img[@title='ODD']";

    private const string SplitSelector = "//img[@title='Split 0, 00']";

    private const string ThirtyFourSelector = "//img[@title='Straight up 34']";

    // <img src="https://dcdn2.lordswm.com/i/roul/kd.png" onclick="putbet(this)" alt="" title="Sixline 7-12" width="12" height="12" onmouseover="ch(this)" class="" style="cursor: pointer;">
    private const string SevenSixline = "//img[@title='Sixline 7-12']";

    // <img src="https://dcdn.lordswm.com/i/roul/kr.png" onclick="putbet(this)" alt="" title="Numbers 0, 00, 1, 2, 3" width="12" height="12" onmouseover="ch(this)" class="" style="cursor: pointer;">
    private const string ZeroSixline = "//img[@title='Numbers 0, 00, 1, 2, 3']";

    public void SelectBetNumbers(string selector)
    {
        Thread.Sleep(SleepTime);
        var roulettePoint = Awaiter.Until(x => x.FindElement(By.XPath(selector)));
        roulettePoint.Click();
        Thread.Sleep(SleepTime);
    }

    public void Input(decimal bet)
    {
        Thread.Sleep(SleepTime);
        // <input type="text" name="bet" size="4" value="0" alt="" title="Stake" maxlength="5" style="width:72px;">
        var input = Awaiter.Until(x => x.FindElement(By.XPath("//input[@name='bet' and @type='text']")));
        input.Clear();
        input.SendKeys(((int)bet).ToString());
        Thread.Sleep(SleepTime);
    }

    public void SubmitBet()
    {
        Thread.Sleep(SleepTime);
        // <input type="submit" value="Bet!" onclick="return checkbet();">
        var submit = Awaiter.Until(x => x.FindElement(By.XPath("//input[@type='submit' and @value='Bet!']")));
        submit.Click();
        Thread.Sleep(SleepTime);
    }

    private bool CheckIfWarning(string warning)
    {
        Thread.Sleep(SleepTime);

        // <h2>Please choose a tile or crossing to bet on</h2>
        var elements = Awaiter.Until(x => x.FindElements(By.XPath($"//h2[text()='{warning}']")));
        if (elements.Any())
        {
            var h2 = elements.First();
            return h2.Displayed && h2.Enabled;
        }

        return false;
    }


    public void MakeBets()
    {
        Driver.Navigate().GoToUrl(GameRouletteUrl);

        var minimalBet = (decimal)RouletteManager.Bet;
        var bet = minimalBet;

        RouletteManager.Mark(Markers.Started);

        if (!RouletteManager.HasMarker(Markers.Number))
        {
            SelectBetNumbers(ThirtyFourSelector);
            Input(bet);
            SubmitBet();
            if (CheckIfWarning(SelectTileWarning) || CheckIfWarning(IncorrectBetWarning))
            {
                Console.WriteLine("No zone selected warning.");
                throw new InvalidOperationException("No zone selected warning.");
            }

            RouletteManager.Mark(Markers.Number);
            Console.WriteLine($"{nameof(ThirtyFourSelector)} : {bet}");
        }

        RouletteManager.Mark(Markers.Finished);
    }

    public string GetLastWinningNumber()
    {
        Driver.Navigate().GoToUrl(GameRouletteResultsUrl);

        var numberElement = Awaiter.Until(x => x.FindElement(By.XPath("//center/table/tbody/tr/td/table/tbody/tr/td/table/tbody/tr[2]/td[3]/font/b")));
        var numberText = numberElement.Text;
        Console.WriteLine($"Read number: {numberText}.");
        return numberText;
    }
}
