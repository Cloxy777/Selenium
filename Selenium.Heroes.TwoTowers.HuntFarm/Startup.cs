using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Selenium.Heroes.CardCollector;

public class HuntInfo
{
    public CreatureInfo CreatureInfo { get; set; } = default!;

    public RewardInfo RewardInfo { get; set; } = default!;

    public int Repeat { get; set; } = 0;

    public int Order { get; set; } = -1;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public record CreatureInfo(string Name, int Count);

public record RewardInfo(int Points, int Gold);

public class Startup
{
    public const string RewardsFullPath = @"..\..\..\rewards.json";
    public const string MaxPointsFullPath = @"..\..\..\max_points.json";

    public static void Run() 
    {
        var engine = new HeroesHuntEngine();
        engine.Authenticate();
   
        var jsonContent = File.ReadAllText(RewardsFullPath);
        var values = JsonConvert.DeserializeObject<List<HuntInfo>>(jsonContent) ?? throw new Exception("Rewards not parsed.");
        Console.WriteLine($"Rewards loaded. Count: {values.Count}.");

        jsonContent = File.ReadAllText(MaxPointsFullPath);
        var maxPoints = JsonConvert.DeserializeObject<int?>(jsonContent) ?? throw new Exception("Max points not parsed.");
        Console.WriteLine($"Max points loaded. {maxPoints}.");

        var seconds = 20;
        while (true)
        {
            values = Filter(values, x => x.Timestamp >= DateTime.UtcNow.AddDays(-1));

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

            var creatureName = string.Empty;
            var creatureCount = 0;

            var found = false;
            foreach (string template in StartPhraseTemplates)
            {
                if (Regex.IsMatch(text, template))
                {
                    var match = Regex.Match(text, template);
                    creatureName = match.Groups[1].Value;
                    
                    if (int.TryParse(match.Groups[2].Value, out var value))
                    {
                        creatureCount = value;
                    }

                    found = true;
                    break;
                }
            }

            if (!found) throw new InvalidOperationException("Creatures NOT parsed !!!!!");

            var creatureInfo = new CreatureInfo(creatureName, creatureCount);

            StoreReward(creatureInfo, points, gold, text, values);

            if (IsGoodReward(values, creatureInfo, maxPoints))
            {
                values = Filter(values, x => !x.CreatureInfo.Equals(creatureInfo));
                Save(values, RewardsFullPath);
                Console.WriteLine($"Rewards file overwritten.");

                Console.WriteLine("Exit");
                break;
            }

            // over half hunts repeated more then 1 time.
            var duplicatesCount = values.Count(x => x.Repeat > 1);
            Console.WriteLine($"Duplicates: {duplicatesCount} > {values.Count / 2}.");
            if (duplicatesCount > values.Count / 2)
            {
                maxPoints--;
                values = SetRepeats(values);
                Save(maxPoints, MaxPointsFullPath);
                Console.WriteLine($"Max points file overwritten. {maxPoints}");
            }

            values = SetOrders(values);
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

    private static void StoreReward(CreatureInfo creatureInfo, int points, int gold, string text, List<HuntInfo> values)
    {
        var reward = new RewardInfo(points, gold);

        if (!values.Any(x => x.CreatureInfo.Equals(creatureInfo)))
        {
            values.Add(new HuntInfo { CreatureInfo = creatureInfo, RewardInfo = reward });
        }

        // new or existing.
        var value = values.First(x => x.CreatureInfo.Equals(creatureInfo));
        value.Repeat++;
        Console.WriteLine($"{creatureInfo}. {reward}. Repeat: {value.Repeat}. Timestamp: {value.Timestamp}.");
    }

    private static bool IsGoodReward(List<HuntInfo> values, CreatureInfo creatureInfo, int maxPoints)
    {
        var topRewards = values
            .Where(x => x.RewardInfo.Points >= maxPoints)
            .OrderByDescending(x => x.RewardInfo.Points)
            .ThenByDescending(x => x.RewardInfo.Gold)
            .ToList();

        return topRewards.Any(x => x.CreatureInfo.Equals(creatureInfo));
    }

    private static List<HuntInfo> Filter(List<HuntInfo> values, Func<HuntInfo, bool> func)
    {      
        values = values
            .Where(func)
            .OrderByDescending(x => x.RewardInfo.Points)
            .ThenByDescending(x => x.RewardInfo.Gold)
            .ToList();

        values = SetOrders(values);

        return values;
    }

    private static List<HuntInfo> SetOrders(List<HuntInfo> values)
    {
        var i = 1;
        foreach (var value in values)
        {
            value.Order = i++;
        }

        return values;
    }

    private static List<HuntInfo> SetRepeats(List<HuntInfo> values)
    {
        foreach (var value in values)
        {
            value.Repeat = 1;
        }

        return values;
    }

    private static int GetMaxPoints(Dictionary<string, RewardInfo> values, ref int maxPoints)
    {
        var actualMaxPoints = values.MaxBy(x => x.Value.Points).Value.Points;

        if (actualMaxPoints > maxPoints)
        {
            Console.WriteLine($"FOUND MAX POINTS -----------------> {actualMaxPoints}.");
            maxPoints = actualMaxPoints;
        }

        return maxPoints;
    }

    private static string CreatureTemplate => @"([A-Z][a-z]+\s?[a-z]*) \((\d+)\)";

    private static string[] StartPhraseTemplates => new[]
    {
        @$"You hear a scream of {CreatureTemplate}",
        @$"You notice {CreatureTemplate}",
        @$"You notice traces leading to a camp of {CreatureTemplate}",
        @$"You smell {CreatureTemplate}"
    };
}
