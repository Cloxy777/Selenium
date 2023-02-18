using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Extensions;

public static class ResourceTypeExtensions
{

    public static int GetResourceValue(this ResourceType resourceType, Player player)
    {
        switch (resourceType)
        {
            case ResourceType.Ore: return player.Ore;
            case ResourceType.Mana: return player.Mana;
            case ResourceType.Stacks: return player.Stacks;
            case ResourceType.Mines: return player.Mines;
            case ResourceType.Monasteries: return player.Monasteries;
            case ResourceType.Barracks: return player.Barracks;
            case ResourceType.Wall: return player.Wall;
            case ResourceType.Tower: return player.Tower;
            default: throw new NotSupportedException($"{nameof(GetResourceValue)} not support {resourceType} resource type.");
        }
    }

    public static int GetProductionDependentResourceValue(this ResourceType resourceType, Player player)
    {
        switch (resourceType)
        {
            case ResourceType.Mines: return player.Ore;
            case ResourceType.Monasteries: return player.Mana;
            case ResourceType.Barracks: return player.Stacks;
            default: throw new NotSupportedException($"{nameof(GetProductionDependentResourceValue)} not support {resourceType} resource type.");
        }
    }

    public static string GetRegexTemplate(this ResourceType enumValue)
    {
        switch (enumValue)
        {
            case ResourceType.Mines:
            case ResourceType.Monasteries:
            case ResourceType.Barracks:
                return @"^.*([-+]\d+)\D+({1})\D+({0}).*$";

            case ResourceType.Ore:
            case ResourceType.Mana:
            case ResourceType.Stacks:
                return @"^.*([-+]\d+).+({0})\D*{1}.*$";
                
            case ResourceType.Tower:
            case ResourceType.Wall:
                return @"^.*([-+]\d+).+(?!.*damage).*({1})\D*{0}.*$";
            default: throw new NotImplementedException();
        }
    }

    public static string[] GetDescriptions(this ResourceType enumValue)
    {
        switch (enumValue)
        {
            case ResourceType.Mines:
                return new string[] { "mine", "mines" };
            case ResourceType.Ore:
                return new string[] { "ore" };
            case ResourceType.Monasteries:
                return new string[] { "monastery", "monasteries" };
            case ResourceType.Mana:
                return new string[] { "mana" };
            case ResourceType.Barracks:
                return new string[] { "barracks" };
            case ResourceType.Stacks:
                return new string[] { "stacks" };
            case ResourceType.Tower:
                return new string[] { "tower" };
            case ResourceType.Wall:
                return new string[] { "wall", "walls" };
            default: throw new NotImplementedException();
        }
    }

    public static string[] GetDescriptions(this Side side, ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Ore:
            case ResourceType.Mana:
            case ResourceType.Stacks:
                switch (side)
                {
                    case Side.Player:
                        return new string[] { "to all players", "" };
                    case Side.Enemy:
                        return new string[] { "to all players", "to enemy" };
                    default: throw new NotImplementedException();
                }
            case ResourceType.Mines:
            case ResourceType.Monasteries:
            case ResourceType.Barracks:
            case ResourceType.Tower:
            case ResourceType.Wall:
                switch (side)
                {
                    case Side.Player:
                        return new string[] { "to all", "" };
                    case Side.Enemy:
                        return new string[] { "to all", "to enemy" };
                    default: throw new NotImplementedException();
                }
            default: throw new NotImplementedException();
        }
    }
}
