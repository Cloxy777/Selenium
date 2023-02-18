using bestcaptchasolver;
using Selenium.Heroes.Common.Configuration;

namespace Selenium.Heroes.Worker;

public class CaptchaResolver
{
    public static BestCaptchaSolverAPI bcs = new BestCaptchaSolverAPI(HeroesConfiguration.CaptchaResolverOptions.AccessToken);

    public static string GetCaptchaId(string path)
    {
        var balance = bcs.account_balance();
        Console.WriteLine(string.Format("Balance: {0}", balance));

        Console.WriteLine("Solving image captcha ...");
        var d = new Dictionary<string, string>
        {
            { "image", path },
            { "minlength ", "6" },
            { "maxlength", "6" }
        };

        var id = bcs.submit_image_captcha(d);
        return id;
    }

    public static string GetCaptchaText(string id)
    {
        var text = "";
        do
        {
            Thread.Sleep(5000);
            text = bcs.retrieve(id)["text"];          
        } while (text == "");

        return text;
    }

    public static void SetBadCaptcha(string id)
    {
        bcs.set_captcha_bad(id);
        Thread.Sleep(5000);
    }
}
