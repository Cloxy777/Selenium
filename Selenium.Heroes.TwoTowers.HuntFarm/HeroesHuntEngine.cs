using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.WebDriver.UndetectedChromeDriver;
using Sl.Selenium.Extensions.Chrome;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text.RegularExpressions;
using OpenQA.Selenium.DevTools.V107.CacheStorage;
using System.Xml.Linq;
using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.CardCollector;

public class HeroesHuntEngine : HeroesEngineBase
{
    public string GetHuntText()
    {
        Driver.Navigate().GoToUrl(GameMiningUrl);

        var div = Awaiter.Until(x => x.FindElement(By.XPath("//div[@id='neut_right_block']")));
        var textDiv = div.FindElement(By.XPath("./div[1]/div[1]"));
        return textDiv.Text;     
    }

    public void SearchAnotherHunt()
    {
        Driver.Navigate().GoToUrl(GameMiningUrl);

        var div = Awaiter.Until(x => x.FindElement(By.XPath("//div[@id='hunt_but_cancel']")));
        div.Click();
    }
}
