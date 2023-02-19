using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Selenium.WebDriver.UndetectedChromeDriver;
using Sl.Selenium.Extensions.Chrome;
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

    private readonly IWebDriver _driver;

    private readonly IWait<IWebDriver> _awaiter;

    public HeroesEngineBase()
    {
        UndetectedChromeDriver.KillAllChromeProcesses();
        UndetectedChromeDriver.ENABLE_PATCHER = true;

        var @params = new ChromeDriverParameters()
        {
            Timeout = TimeSpan.FromSeconds(10),
            ProfileName = "bordox",
            DriverArguments = Arguments
        };

        _driver = UndetectedChromeDriver.Instance(@params);

        _awaiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
    }

    public IWebDriver Driver => _driver;

    public IWait<IWebDriver> Awaiter => _awaiter;

    protected virtual HashSet<string> Arguments => new HashSet<string>
    {
        "--no-sandbox",
        "--window-size=1920,1080",
        //"--headless-new",
        "--disable-gpu",
        "--allow-running-insecure-content"
    };

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

    public void NavigateToLocal()
    {
        Driver.Navigate().GoToUrl(LocalUrl);
    }

    public void NavigateToMain()
    {
        Driver.Navigate().GoToUrl(GameMainUrl);
    }
}
