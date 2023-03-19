using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using Selenium.Heroes.Common.Configuration;

namespace Selenium.Heroes.Common;

public abstract class HeroesEngineBase
{
    protected const string LocalUrl = "file:\\C:\\Users\\Pavel\\Documents\\Tavern. Card games..html";

    protected const string GameMainUrl = "https://www.lordswm.com/home.php?info";

    protected const string GameMapUrl = "https://www.heroeswm.com/map.php";

    protected const string GameMiningUrl = "https://www.heroeswm.com/map.php?st=mn";

    protected const string GameTavernUrl = "https://www.lordswm.com/tavern.php";

    protected const string GameRouletteUrl = "https://www.lordswm.com/roulette.php";

    protected const string GameRouletteResultsUrl = "https://www.lordswm.com/allroul.php";

    public IWebDriver Driver => WebDriverSingleton.Driver;

    public IWait<IWebDriver> Awaiter => WebDriverSingleton.Awaiter;

    public void Authenticate()
    {
        Driver.Navigate().GoToUrl(GameMapUrl);

        if (IsLoginPage())
        {
            Console.WriteLine("Login...");
            Login();
            Console.WriteLine("Login done.");
        }
    }


    private bool IsLoginPage()
    {
        var elements = Awaiter.Until(x => x.FindElements(By.XPath("//input[@name = 'login']")));
        return elements.Any();
    }

    private void Login()
    {
        var loginInput = Awaiter.Until(x => x.FindElement(By.XPath("//input[@name = 'login']")));
        loginInput.Clear();

        var username = HeroesConfiguration.HeroesEngineOptions.Credentials.UserName;
        loginInput.SendKeys(username);

        var passwordInput = Awaiter.Until(x => x.FindElement(By.XPath("//input[@name = 'pass']")));
        passwordInput.Clear();

        var password = HeroesConfiguration.HeroesEngineOptions.Credentials.Password;
        passwordInput.SendKeys(password);

        var enterButton = Awaiter.Until(x => x.FindElement(By.XPath("//div[@class = 'entergame']/input[@type = 'image']")));
        enterButton.Click();
    }

    public bool IsCardGamePage()
    {
        var actualUrl = Driver.Url;

        var regex = @"https:\/\/www\.lordswm\.com\/cgame\.php\?gameid=";

        var match = Regex.Match(actualUrl, regex, RegexOptions.IgnoreCase);

        return match.Success;
    }


    public bool IsTavernPage()
    {
        var actualUrl = Driver.Url;

        var regex = @$"{GameTavernUrl}";

        var match = Regex.Match(actualUrl, regex, RegexOptions.IgnoreCase);

        return match.Success;
    }

    public void NavigateToLocal()
    {
        Driver.Navigate().GoToUrl(LocalUrl);
    }

    public void NavigateToMain()
    {
        Driver.Navigate().GoToUrl(GameMainUrl);
    }
}
