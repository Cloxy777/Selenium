using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Selenium.WebDriver.UndetectedChromeDriver;
using Sl.Selenium.Extensions.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Heroes.Common;

public static class WebDriverSingleton
{
    private static readonly IWebDriver _driver;

    private static readonly IWait<IWebDriver> _awaiter;

    static WebDriverSingleton()
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

    public static IWebDriver Driver => _driver;

    public static IWait<IWebDriver> Awaiter => _awaiter;

    public static HashSet<string> Arguments => new HashSet<string>
    {
        "--no-sandbox",
        "--window-size=1920,1080",
        //"--headless-new",
        "--disable-gpu",
        "--allow-running-insecure-content"
    };
}
