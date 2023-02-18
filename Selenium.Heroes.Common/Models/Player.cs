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
                break;
            case ResourceType.Monasteries:
                player.Monasteries = player.Monasteries + resourceEffect.Value;
                break;
            case ResourceType.Mana:
                player.Mana = player.Mana + resourceEffect.Value;
                break;
            case ResourceType.Barracks:
                player.Barracks = player.Barracks + resourceEffect.Value; 
                break;
            case ResourceType.Stacks:
                player.Stacks = player.Stacks + resourceEffect.Value;
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

    public Player Produce(ResourceType resourceType)
    {
        var player = new Player(this);

        switch (resourceType)
        {
            case ResourceType.Mines:
                player.Ore = player.Ore + player.Mines;
                break;
            case ResourceType.Monasteries:
                player.Mana = player.Mana + player.Monasteries;
                break;
            case ResourceType.Barracks:
                player.Stacks = player.Stacks + player.Barracks;
                break;
            default:
                throw new NotSupportedException($"{nameof(Produce)} not support {resourceType} resource type.");
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

    public int GetResourceValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Ore: return Ore;
            case ResourceType.Mana: return Mana;
            case ResourceType.Stacks: return Stacks;
            case ResourceType.Mines: return Mines;
            case ResourceType.Monasteries: return Monasteries;
            case ResourceType.Barracks: return Barracks;
            case ResourceType.Wall: return Wall;
            case ResourceType.Tower: return Tower;
            default: throw new NotSupportedException($"{nameof(GetResourceValue)} not support {resourceType} resource type.");
        }
    }

    public override string ToString()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        return json;
    }
}


