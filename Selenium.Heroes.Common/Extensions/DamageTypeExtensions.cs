using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.Extensions;

public static class DamageTypeExtensions
{
    public static string GetRegexTemplate(this DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Pure:
                return @"^\s*(\d+)\s*{1}\s*$";
            case DamageType.Tower:
                return @"(\d+).+{1}.+{0}";
            default: throw new NotImplementedException();
        }
    }

    public static string[] GetDescriptions(this DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Pure:
                return new string[] { "" };
            case DamageType.Tower:
                return new string[] { "tower" };
            default: throw new NotImplementedException();
        }
    }

    public static string[] GetDescriptions(this Side side, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Tower:
                switch (side)
                {
                    case Side.Player:
                        return new string[] { "damage to all", "damage to your" };
                    case Side.Enemy:
                        return new string[] { "damage to all", "damage to enemy" };
                    default: throw new NotImplementedException();
                }
            case DamageType.Pure:
                switch (side)
                {
                    case Side.Player:
                        return new string[] { "damage" };
                    case Side.Enemy:
                        return new string[] { "damage to enemy" };
                    default: throw new NotImplementedException();
                }
            default: throw new NotImplementedException();
        }
    }
}
