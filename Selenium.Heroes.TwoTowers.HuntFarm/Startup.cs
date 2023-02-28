
using Newtonsoft.Json;
using Selenium.Heroes.Common.Models;
using Selenium.Heroes.Common;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;

namespace Selenium.Heroes.CardCollector;

public record HuntReward(int Points, int Gold);

public class Startup
{
    public const string RewardsFullPath = "E:\\GitHub\\Selenium\\Selenium.Heroes.TwoTowers.HuntFarm\\rewards.json";

    public const int MaxCount = 30;

    public static void Run() 
    {
        var engine = new HeroesHuntEngine();

        var jsonContent = File.ReadAllText(RewardsFullPath);
        var values = JsonConvert.DeserializeObject<Dictionary<string, HuntReward>>(jsonContent) ?? throw new Exception("Rewards not parsed.");
        Console.WriteLine($"Rewards loaded. Count: {values.Count}.");

        if (IsEnoughValues(values))
        {
            values = values
                .OrderByDescending(x => x.Value.Points)
                .ThenByDescending(x => x.Value.Gold)
                .Take(MaxCount)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        var seconds = 20;
        while (true)
        {
            var text = engine.GetHuntText();
            Console.WriteLine($"Text: {text}");

            var points = 1;
            var pattern = @"\+(\d) HG points";
            if (Regex.IsMatch(text, pattern))
            {
                var match = Regex.Match(text, pattern);
                foreach (var group in match.Groups.Values)
                {
                    if (int.TryParse(group.Value, out var value))
                    {
                        points = value;
                        break;
                    }
                }
            }

            var gold = 0;
            pattern = @"(\d+) gold";
            if (Regex.IsMatch(text, pattern))
            {
                var match = Regex.Match(text, pattern);
                foreach (var group in match.Groups.Values)
                {
                    if (int.TryParse(group.Value, out var value))
                    {
                        gold = value;
                        break;
                    }
                }
            }

            values[text] = new HuntReward(points, gold);
            Console.WriteLine($"Hunt points: {points}. Gold: {gold}.");

            if (!IsEnoughValues(values))
            {
                engine.SearchAnotherHunt();
                Thread.Sleep(seconds * 1000);
                continue;
            }

            if (IsGoodReward(values, text))
            {
                values = values
                    .Where(x => x.Key != text)
                    .OrderByDescending(x => x.Value.Points)
                    .ThenByDescending(x => x.Value.Gold)
                    .ToDictionary(x => x.Key, x => x.Value);

                var settings = new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                };
                var json = JsonConvert.SerializeObject(values, Formatting.Indented, settings);
                File.WriteAllText(RewardsFullPath, json);
                Console.WriteLine($"Rewards file overwritten.");


                Console.WriteLine("Exit");
                break;
            }

            engine.SearchAnotherHunt();
            Thread.Sleep(seconds * 1000);
            continue;
        }     
    }

    private static bool IsGoodReward(Dictionary<string, HuntReward> values, string text)
    {
        var topThreeTextRewards = values
            .OrderByDescending(x => x.Value.Points)
            .ThenByDescending(x => x.Value.Gold)
            .Select(x => x.Key)
            .Take(3)
            .ToList();

        return topThreeTextRewards.Contains(text);
    }

    private static bool IsEnoughValues(Dictionary<string, HuntReward> values)
    {
        return values.Count >= MaxCount;
    }
}
