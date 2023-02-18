namespace Selenium.Heroes.Common.Configuration;

public class HeroesEngineOptions
{
    public Credentials Credentials { get; set; } = default!;
}

public class Credentials
{
    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}