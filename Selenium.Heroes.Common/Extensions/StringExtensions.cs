namespace Selenium.Heroes.Common.Extensions;

public static class StringExtensions
{
    public static bool NotEmpty(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}
