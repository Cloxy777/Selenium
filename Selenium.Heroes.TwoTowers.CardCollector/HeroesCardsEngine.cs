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

public class HeroesCardsEngine : HeroesEngineBase
{
    public List<Card> GetAllCards()
    {
        //const string LocalUrl = "file:\\C:\\Users\\Pavel\\Documents\\Card game. Lords of War and Money..html";
        //Driver.Navigate().GoToUrl(LocalUrl);

        SaveScreenshot(StringConstants.CardGameFileName);

        var cardDivs = Awaiter.Until(x => x.FindElements(By.XPath("//div[@class='card']")));

        var cards = new List<Card>();

        foreach (var cardDiv in cardDivs)
        {
            var card = ParseCard(cardDiv);

            if (card != null)
            {
                cards.Add(card);
            }  
        }

        return cards;
    }

    private void SaveScreenshot(string fileName)
    {
        var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
    }

    private Card? ParseCard(IWebElement element)
    {
        var innerHtml = element.GetAttribute("innerHTML");
        var card = new Card();
        try
        {
            var isDisabled = element.FindElements(By.XPath(".//div[contains(@class, 'cardDisabled')]")).Any();

            var isRed = element.FindElements(By.XPath(".//div[contains(@class, 'cardRed')]")).Any();
            var isBlue = element.FindElements(By.XPath(".//div[contains(@class, 'cardBlue')]")).Any();
            var isGreen = element.FindElements(By.XPath(".//div[contains(@class, 'cardGreen')]")).Any();

            card.CardType = isRed ? CardType.Ore : isBlue ? CardType.Mana : isGreen ? CardType.Stacks : throw new Exception("Undefined card type.");

            card.Header = element.FindElement(By.XPath(".//div[contains(@class, 'cardContentHead')]/span")).Text;

            var value = element.FindElement(By.XPath(".//div[contains(@class, 'cardValue')]/span")).Text;
            card.Cost = Convert.ToInt32(value);

            card.Description = element.FindElement(By.XPath(".//div[contains(@class, 'cardText')]/div[contains(@class, 'cardContentDescr')]")).Text;

            if (!isDisabled && card.Header.NotEmpty())
            {
                var fileName = $"{(int)card.CardType}_{card.Cost}_{card.Header}";
                SaveCardImage(element, StringConstants.CardGameFileName, fileName);
            }

            return card;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private void SaveCardImage(IWebElement element, string screenShotName, string fileName)
    {
        Directory.CreateDirectory(StringConstants.CardsFullPathDirectory);

        using var image = Bitmap.FromFile(screenShotName);
        var rectangle = new Rectangle();

        if (element != null)
        {
            // Get the Width and Height of the WebElement using
            var width = element.Size.Width;
            var height = element.Size.Height;

            // Get the Location of WebElement in a Point.
            // This will provide X & Y co-ordinates of the WebElement
            var p = element.Location;

            // Create a rectangle using Width, Height and element location
            rectangle = new Rectangle(p.X, p.Y, width, height);
        }

        // croping the image based on rect.
        using var bitmap = new Bitmap(image);

        using var cardImage = bitmap.Clone(rectangle, bitmap.PixelFormat);

        var path = StringConstants.CardsFullPathDirectory + "\\" + fileName + ".png";

        if (!File.Exists(path))
        {
            ImageHelper.SaveImage(cardImage, path);
        }
    }
}
