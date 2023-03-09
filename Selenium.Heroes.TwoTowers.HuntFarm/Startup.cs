using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;


namespace Selenium.Heroes.CardCollector;

public record HuntReward(int Points, int Gold, int Order = 0);

public class Startup
{
    public const string RewardsFullPath = "E:\\GitHub\\Selenium\\Selenium.Heroes.TwoTowers.HuntFarm\\rewards.json";

    public static void Run() 
    {
        var engine = new HeroesHuntEngine();
        engine.Authenticate();

        var jsonContent = File.ReadAllText(RewardsFullPath);
        var values = JsonConvert.DeserializeObject<Dictionary<string, HuntReward>>(jsonContent) ?? throw new Exception("Rewards not parsed.");
        Console.WriteLine($"Rewards loaded. Count: {values.Count}.");
        
        values = Filter(values, x => x.Value.Points >= 3);

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

            if (IsGoodReward(values, text))
            {
                values = Filter(values, x => x.Key != text);

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
        values = Filter(values, x => x.Value.Points >= 3);

        var topRewards = values
            .OrderByDescending(x => x.Value.Points)
            .ThenByDescending(x => x.Value.Gold)
            .Select(x => x.Key)
            .Take(values.Count / 2)
            .ToList();

        return topRewards.Contains(text);
    }

    private static Dictionary<string, HuntReward> Filter(Dictionary<string, HuntReward> values, Func<KeyValuePair<string, HuntReward>, bool> func)
    {
        var i = 1;
        return values
            .Where(x => x.Value.Points >= 3)
            .OrderByDescending(x => x.Value.Points)
            .ThenByDescending(x => x.Value.Gold)
            .ToDictionary(x => x.Key, x => new HuntReward(x.Value.Points, x.Value.Gold, i++));
    }
}
