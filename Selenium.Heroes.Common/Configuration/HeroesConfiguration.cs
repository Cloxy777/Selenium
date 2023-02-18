using Microsoft.Extensions.Configuration;

namespace Selenium.Heroes.Common.Configuration;

public class HeroesConfiguration
{
    public static HeroesEngineOptions HeroesEngineOptions { get; private set; } = default!;

    public static CaptchaResolverOptions CaptchaResolverOptions { get; private set; } = default!;

    public static void ReadConfiguration()
    {
        var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile($"appsettings.json", true, true);

        var config = builder.Build();

        HeroesEngineOptions = config.GetSection("HeroesEngineOptions").Get<HeroesEngineOptions>() ?? new HeroesEngineOptions();
        CaptchaResolverOptions = config.GetSection("CaptchaResolverOptions").Get<CaptchaResolverOptions>() ?? new CaptchaResolverOptions();
    }
}
