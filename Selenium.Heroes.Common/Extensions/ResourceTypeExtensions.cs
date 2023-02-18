using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Extensions;

public static class ResourceTypeExtensions
{
    public static ResourceType ToProductionType(this ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Ore: return ResourceType.Mines;
            case ResourceType.Mana: return ResourceType.Monasteries;
            case ResourceType.Stacks: return ResourceType.Barracks;
            default: throw new NotSupportedException($"{nameof(ToProductionType)} not support {resourceType} resource type.");
        }
    }

    public static ResourceType ToProducedType(this ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Mines: return ResourceType.Ore;
            case ResourceType.Monasteries: return ResourceType.Mana;
            case ResourceType.Barracks: return ResourceType.Stacks;
            default: throw new NotSupportedException($"{nameof(ToProducedType)} not support {resourceType} resource type.");
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
