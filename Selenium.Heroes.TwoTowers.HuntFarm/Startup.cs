using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;

namespace Selenium.Heroes.CardCollector;

public record HuntReward(int Points, int Gold, int Count, DateTime Timestamp, int Order = -1);

public class Startup
{
    public const string RewardsFullPath = @"..\..\..\rewards.json";
    public const string MaxPointsFullPath = @"..\..\..\max_points.json";

    public static void Run() 
    {
        var engine = new HeroesHuntEngine();
        engine.Authenticate();
   
        var jsonContent = File.ReadAllText(RewardsFullPath);
        var values = JsonConvert.DeserializeObject<Dictionary<string, HuntReward>>(jsonContent) ?? throw new Exception("Rewards not parsed.");
        values = Filter(values, x => x.Value.Timestamp >= DateTime.UtcNow.AddDays(-1));
        Console.WriteLine($"Rewards loaded. Count: {values.Count}.");

        jsonContent = File.ReadAllText(MaxPointsFullPath);
        var maxPoints = JsonConvert.DeserializeObject<int?>(jsonContent) ?? throw new Exception("Max points not parsed.");
        Console.WriteLine($"Max points loaded. Count: {maxPoints}.");

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

            StoreReward(points, gold, text, values);

            if (IsGoodReward(values, text, maxPoints))
            {
                values = Filter(values, x => x.Key != text);

                Save(values, RewardsFullPath);
                Console.WriteLine($"Rewards file overwritten.");

                Console.WriteLine("Exit");
                break;
            }

            if (values.All(x => x.Value.Count > 1))
            {
                maxPoints--;
                Save(maxPoints, MaxPointsFullPath);
                Console.WriteLine($"Max points file overwritten.");
            }

            Save(values, RewardsFullPath);
            Console.WriteLine($"Rewards file overwritten.");

            engine.SearchAnotherHunt();
            Thread.Sleep(seconds * 1000);
            continue;
        }     
    }

    public static void Save(object @object, string path)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = { new StringEnumConverter() }
        };
        var json = JsonConvert.SerializeObject(@object, Formatting.Indented, settings);
        File.WriteAllText(path, json);
    }

    private static void StoreReward(int points, int gold, string text, Dictionary<string, HuntReward> values)
    {
        var timestamp = DateTime.UtcNow;
        var reward = new HuntReward(points, gold, 0, timestamp);

        if (!values.ContainsKey(text))
        {
            values[text] = reward;
        }

        // new or existing.
        reward = values[text];

        var count = reward.Count + 1;
        values[text] = new HuntReward(reward.Points, reward.Gold, count, timestamp);
        Console.WriteLine($"Hunt points: {reward.Points}. Gold: {reward.Gold}. Count: {count}. Timestamp: {timestamp}.");

        //var reward = new HuntReward(points, gold);
        //Console.Write($"Hunt points: {reward.Points}. Gold: {reward.Gold}");
        //if (IsMaxPoints(reward, values))
        //{
        //    values[text] = reward;
        //    Console.WriteLine(" added.");
        //}
        //else
        //{
        //    Console.WriteLine(".");
        //}
    }

    private static bool IsGoodReward(Dictionary<string, HuntReward> values, string text, int maxPoints)
    {

        var topRewards = values
            .Where(x => x.Value.Points >= maxPoints)
            .OrderByDescending(x => x.Value.Points)
            .ThenByDescending(x => x.Value.Gold)
            .Select(x => x.Key)
            .ToList();

        return topRewards.Contains(text);
    }

    private static Dictionary<string, HuntReward> Filter(Dictionary<string, HuntReward> values, Func<KeyValuePair<string, HuntReward>, bool> func)
    {
        var i = 1;
        return values
            .Where(func)
            .OrderByDescending(x => x.Value.Points)
            .ThenByDescending(x => x.Value.Gold)
            .ToDictionary(x => x.Key, x => new HuntReward(x.Value.Points, x.Value.Gold, i++, x.Value.Timestamp));
    }

    private static int GetMaxPoints(Dictionary<string, HuntReward> values, ref int maxPoints)
    {
        var actualMaxPoints = values.MaxBy(x => x.Value.Points).Value.Points;

        if (actualMaxPoints > maxPoints)
        {
            Console.WriteLine($"FOUND MAX POINTS -----------------> {actualMaxPoints}.");
            maxPoints = actualMaxPoints;
        }

        return maxPoints;
    }
}
