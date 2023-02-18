using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.WebDriver.UndetectedChromeDriver;
using Sl.Selenium.Extensions.Chrome;
using System.Drawing;
using System.Text.RegularExpressions;
using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Models;
using Newtonsoft.Json;
using System.Linq;
using Selenium.Heroes.Common.Loaders;
using Selenium.Heroes.Common.CardDescriptors;

namespace Selenium.Heroes.TwoTowers;

public class HeroesTwoTowersEngine : HeroesEngineBase
{
    public Player GetPlayerInfo()
    {
        var player = new Player("Player");

        var gameContainer = Awaiter.Until(x => x.FindElement(By.XPath("//div[@id='gameContainer']")));

        // ORE
        var oreIncreventValue = gameContainer.FindElement(By.XPath(".//div[@id='res1plus_1']")).Text;
        player.Mines = Convert.ToInt32(oreIncreventValue);

        var oreCurrentValue = gameContainer.FindElement(By.XPath(".//div[@id='res1_1']")).Text;
        player.Ore = Convert.ToInt32(oreCurrentValue);

        // MANA
        var manaIncreventValue = gameContainer.FindElement(By.XPath(".//div[@id='res2plus_1']")).Text;
        player.Monasteries = Convert.ToInt32(manaIncreventValue);

        var manaCurrentValue = gameContainer.FindElement(By.XPath(".//div[@id='res2_1']")).Text;
        player.Mana = Convert.ToInt32(manaCurrentValue);

        // STACKS
        var stacksIncreventValue = gameContainer.FindElement(By.XPath(".//div[@id='res3plus_1']")).Text;
        player.Barracks = Convert.ToInt32(stacksIncreventValue);

        var stacksCurrentValue = gameContainer.FindElement(By.XPath(".//div[@id='res3_1']")).Text;
        player.Stacks = Convert.ToInt32(stacksCurrentValue);

        // TOWER + WALL
        var towerHeight = gameContainer.FindElement(By.XPath(".//div[@id='tower_res1']")).Text;
        player.Tower = Convert.ToInt32(towerHeight);

        var wallHeight = gameContainer.FindElement(By.XPath(".//div[@id='wall_res1']")).Text;
        player.Wall = Convert.ToInt32(wallHeight);

        return player;

    }

    public Player GetEnemyInfo()
    {
        var enemy = new Player("Enemy");

        var gameContainer = Awaiter.Until(x => x.FindElement(By.XPath("//div[@id='gameContainer']")));

        // ORE
        var oreIncreventValue = gameContainer.FindElement(By.XPath(".//div[@id='res1plus_2']")).Text;
        enemy.Mines = Convert.ToInt32(oreIncreventValue);

        var oreCurrentValue = gameContainer.FindElement(By.XPath(".//div[@id='res1_2']")).Text;
        enemy.Ore = Convert.ToInt32(oreCurrentValue);

        // MANA
        var manaIncreventValue = gameContainer.FindElement(By.XPath(".//div[@id='res2plus_2']")).Text;
        enemy.Monasteries = Convert.ToInt32(manaIncreventValue);

        var manaCurrentValue = gameContainer.FindElement(By.XPath(".//div[@id='res2_2']")).Text;
        enemy.Mana = Convert.ToInt32(manaCurrentValue);

        // STACKS
        var stacksIncreventValue = gameContainer.FindElement(By.XPath(".//div[@id='res3plus_2']")).Text;
        enemy.Barracks = Convert.ToInt32(stacksIncreventValue);

        var stacksCurrentValue = gameContainer.FindElement(By.XPath(".//div[@id='res3_2']")).Text;
        enemy.Stacks = Convert.ToInt32(stacksCurrentValue);

        // TOWER + WALL
        var towerHeight = gameContainer.FindElement(By.XPath(".//div[@id='tower_res2']")).Text;
        enemy.Tower = Convert.ToInt32(towerHeight);

        var wallHeight = gameContainer.FindElement(By.XPath(".//div[@id='wall_res2']")).Text;
        enemy.Wall = Convert.ToInt32(wallHeight);

        return enemy;
    }

    public List<ICardDescriptor> GetCardDescriptors()
    {
        var cardDivs = Awaiter.Until(x => x.FindElements(By.XPath("//div[@class='cardDiscardBtn_in' and not(contains(@style, 'display: none;'))]/..")));
        var html = cardDivs[0].GetAttribute("innerHTML");

        var headers = new List<string>();
        foreach (var cardDiv in cardDivs)
        {
            var header = GetHeader(cardDiv);

            if (header != null)
            {
                headers.Add(header);
            }
        }

        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Any(header => header == x.BaseCardEffect.Card.Header)).ToList();

        return cardDescriptors;
    }

    private string GetHeader(IWebElement element)
    {
        try
        {
            return element.FindElement(By.XPath(".//div[contains(@class, 'cardContentHead')]/span")).Text;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public void Play(Card card)
    {
        var cardDivs = Awaiter.Until(x => x.FindElements(By.XPath("//div[@class='cardDiscardBtn_in' and not(contains(@style, 'display: none;'))]/..")));

        var headers = new List<string>();
        foreach (var cardDiv in cardDivs)
        {
            var header = GetHeader(cardDiv);

            if (header != null && header == card.Header)
            {
                cardDiv.Click();
            }
        }
    }

    public void Discard(Card card)
    {
        var cardDivs = Awaiter.Until(x => x.FindElements(By.XPath("//div[@class='cardDiscardBtn_in' and not(contains(@style, 'display: none;'))]/..")));

        var headers = new List<string>();
        foreach (var cardDiv in cardDivs)
        {
            var header = GetHeader(cardDiv);

            if (header != null && header == card.Header)
            {
                var discard = cardDiv.FindElement(By.XPath(".//div[@class='cardDiscardBtn']"));
                discard.Click();
            }
        }
    }
}
