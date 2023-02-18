using Newtonsoft.Json;
using Selenium.Heroes.Common.Extensions;

namespace Selenium.Heroes.Common.Models;

public class Player
{
    public Player(string name)
    {
        Name = name;
    }

    public Player(Player player)
    {
        Name = player.Name;
        Mines = player.Mines;
        Monasteries = player.Monasteries;
        Barracks = player.Barracks;
        Ore = player.Ore;
        Mana = player.Mana;
        Stacks = player.Stacks;
        Tower = player.Tower;
        Wall = player.Wall;
    }

    public string Name { get; set; }

    public int Mines { get; set; }
    public int Monasteries { get; set; }
    public int Barracks { get; set; }

    public int Ore { get; set; }
    public int Mana { get; set; }
    public int Stacks { get; set; }

    public int Tower { get; set; }
    public int Wall { get; set; }

    public Player Apply(ResourceEffect resourceEffect)
    {
        var player = new Player(this);

        switch (resourceEffect.ResourceType)
        {
            case ResourceType.Mines:
                player.Mines = player.Mines + resourceEffect.Value;
                break;
            case ResourceType.Ore:
                player.Ore = player.Ore + resourceEffect.Value;
                player.Ore = player.Ore + player.Mines;
                break;
            case ResourceType.Monasteries:
                player.Monasteries = player.Monasteries + resourceEffect.Value;
                break;
            case ResourceType.Mana:
                player.Mana = player.Mana + resourceEffect.Value;
                player.Mana = player.Mana + player.Monasteries;
                break;
            case ResourceType.Barracks:
                player.Barracks = player.Barracks + resourceEffect.Value; 
                break;
            case ResourceType.Stacks:
                player.Stacks = player.Stacks + resourceEffect.Value;
                player.Stacks = player.Stacks + player.Barracks;
                break;
            case ResourceType.Tower:
                player.Tower = player.Tower + resourceEffect.Value;
                break;
            case ResourceType.Wall:
                player.Wall = player.Wall + resourceEffect.Value;
                break;
            default:
                throw new NotSupportedException($"{nameof(Apply)} not support {resourceEffect.ResourceType} resource type.");
        }

        return player;
    }

    public Player Wait()
    {
        var player = new Player(this);

        player.Ore = player.Ore + player.Mines;

        player.Mana = player.Mana + player.Monasteries;

        player.Stacks = player.Stacks + player.Barracks;

        return player;
    }

    public override string ToString()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        return json;
    }
}


