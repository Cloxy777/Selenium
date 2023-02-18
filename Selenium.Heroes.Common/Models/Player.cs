using Newtonsoft.Json;

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

    public override string ToString()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        return json;
    }

    public void ApplyPureDamage(int damage)
    {
        if (damage > Wall)
        {
            var towerDamage = damage - Wall;
            Wall = Wall - damage;
            Tower = Tower - towerDamage;
            return;
        }

        Wall = Wall - damage;
    }
}


