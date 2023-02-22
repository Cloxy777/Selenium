using OpenQA.Selenium;
using System.Text.RegularExpressions;
using Selenium.Heroes.Common;

namespace Selenium.Heroes.Worker;

public class HeroesWorkerEngine : HeroesEngineBase
{
    private DateTime _lastWorkTime = DateTime.MinValue;

    public static bool UseCaptchaControl;

    public int HoursDifferance = -1;

    public int MinutesDifferance = 0;

    public DateTime LastWorkTime =>
        _lastWorkTime == DateTime.MinValue ?
        (_lastWorkTime = GetCalculateLastWorkTime()) :
        _lastWorkTime;

    public void StartWork()
    {
        TryAfter(1);

        Driver.Navigate().GoToUrl(GameMapUrl);

        FindAvailableCell();

        if (WorkExists())
        {
            if (!CaptchaExists())
            {
                var workInput = Awaiter.Until(x => x.FindElement(By.XPath("//input[@id = 'wbtn']")));
                workInput.Click();
                return;
            }

            SaveCaptchaImage();
            ResolveCaptcha();
            return;
        }

        if (ReCaptchaExists())
        {
            var frame = Awaiter.Until(x => x.FindElement(By.XPath("//iframe[contains(@src, 'recaptcha')]")));
            Driver.SwitchTo().Frame(frame);

            Thread.Sleep(1000);
            var recapcha = Awaiter.Until(x => x.FindElement(By.XPath("//span[@id = 'recaptcha-anchor']")));
            recapcha.Click();

            Driver.SwitchTo().ParentFrame();

            Thread.Sleep(1000);
            var submit = Awaiter.Until(x => x.FindElement(By.XPath("//input[@type = 'submit' and @value='Enroll']")));
            submit.Click();

            TryAfter(15);
            return;
        }

        ResetLastWorkTime();
    }

    private void ResetLastWorkTime()
    {
        _lastWorkTime = DateTime.MinValue;
    }

    private void FindAvailableCell()
    {
        for (int i = 2; i >= 0;)
        {
            Driver.Navigate().Refresh();
            var buttons = Awaiter.Until(x => x.FindElements(By.XPath("//div[@id='hwm_map_objects_and_buttons']/div[@class='job_fl_btns_block']/a")));
            buttons[i].Click();

            var j = 0;
            while (true)
            {
                var cells = Awaiter.Until(x => x.FindElements(By.XPath("//a[contains(@href,'object-info.php') and contains(text(),'»»»')]")));
                
                if (j >= cells.Count)
                {
                    break;
                }
                
                cells[j].Click();
                
                if (WorkExists() || ReCaptchaExists() || CaptchaExists())
                {
                    return;
                }

                j++;
                Driver.Navigate().Back();
                continue;
            }

            i--;
        }
    }

    private void TryAfter(int after)
    {
        var passed = (60 - after) * -1;
        _lastWorkTime= DateTime.Now.AddMinutes(passed);
    }

    private bool WorkExists()
    {
        var elements = Awaiter.Until(x => x.FindElements(By.XPath("//input[@id = 'wbtn']")));
        var isDetected = elements.Any();

        if (isDetected)
        {
            Console.WriteLine("Work detected.");
        }

        return isDetected;
    }

    private bool ReCaptchaExists()
    {
        var frame = Awaiter.Until(x => x.FindElement(By.XPath("//iframe[contains(@src, 'recaptcha')]")));
        Driver.SwitchTo().Frame(frame);

        var elements = Awaiter.Until(x => x.FindElements(By.XPath("//span[@id='recaptcha-anchor']")));
        var isDetected = elements.Any();

        Driver.SwitchTo().ParentFrame();

        if (isDetected)
        {
            Console.WriteLine("ReCaptcha detected.");
        }

        return isDetected;
    }

    private bool CaptchaExists()
    {
        var elements = Awaiter.Until(x => x.FindElements(By.XPath("//img[@class = 'getjob_capcha']")));
        var isDetected = elements.Any();

        if (isDetected)
        {
            Console.WriteLine("Captcha detected.");
        }

        return isDetected;
    }

    private void SaveCaptchaImage()
    {
        var captchaImage = Awaiter.Until(x => x.FindElement(By.XPath("//img[@class = 'getjob_capcha']")));
        string url = captchaImage.GetAttribute("src");
        var image = ImageHelper.GetImage(url);
        ImageHelper.SaveImage(image, StringConstants.CaptchaFileName);
    }

    private void ResolveCaptcha()
    {
        var id = CaptchaResolver.GetCaptchaId(StringConstants.CaptchaFileName);
        Console.WriteLine($"Captcha submit. Id: {id}.");

        var text = CaptchaResolver.GetCaptchaText(id);
        Console.WriteLine($"Captcha complete. Text: {text}.");

        UseCaptcha(text);

        var success = CheckCaptcha();

        if (!success)
        {
            CaptchaResolver.SetBadCaptcha(id);
            return;
        }

        ResetLastWorkTime();
    }

    private void UseCaptcha(string text)
    {
        var use = true;
        if (UseCaptchaControl)
        {
            Console.WriteLine("Type 'use' to submit captcha. Otherwise marked as bad and new requested.");
            use = Console.ReadLine() == "use";
        }

        if (use)
        {
            SetCaptcha(text);
            PressToWork();
        }
    }

    private bool CheckCaptcha()
    {
        ResetLastWorkTime();
        var success = LastWorkTime > DateTime.Now.AddMinutes(-55);

        return success;
    }

    private void SetCaptcha(string text)
    {
        Console.WriteLine("Captcha submiting...");

        var captchaInput = Awaiter.Until(x => x.FindElement(By.XPath("//input[@id = 'code']")));
        captchaInput.Click();
        captchaInput.Clear();
        captchaInput.SendKeys(text);
    }

    private void PressToWork()
    {
        var workInput = Awaiter.Until(x => x.FindElement(By.XPath("//input[@id = 'wbtn']")));
        workInput.Click();

        Console.WriteLine("Start to work pressed.");
    }

    private DateTime GetCalculateLastWorkTime()
    {
        Driver.Navigate().GoToUrl(GameMainUrl);

        var elements = Awaiter.Until(x => x.FindElements(By.XPath("//span[contains(., 'Currently employed at:')]")));
        var isDetected = elements.Any();

        if (isDetected)
        {
            var span = elements.Single();
            var text = span.Text;

            var match = Regex.Match(text, @" since (\d{2}):(\d{2})");
            var hours = Convert.ToInt32(match.Groups[1].Value);
            var minutes = Convert.ToInt32(match.Groups[2].Value);

            var result = DateTime.Now.Date;
            result = result.AddHours(hours).AddHours(HoursDifferance);
            result = result.AddMinutes(minutes).AddMinutes(MinutesDifferance);

            return result;
        }

        elements = Awaiter.Until(x => x.FindElements(By.XPath("//span[contains(., 'Last work location:')]")));
        isDetected = elements.Any();

        if (isDetected)
        {
            var span = elements.Single();
            var text = span.Text;

            var match = Regex.Match(text, @"in (\d{1,2}) min.");
            var minutes = Convert.ToInt32(match.Groups[1].Value);
            var passed = -1 * (60 - minutes);

            var result = DateTime.Now.Date;
            result = result.AddHours(DateTime.Now.Hour);
            result = result.AddMinutes(DateTime.Now.Minute).AddMinutes(passed);

            return result;
        }

        return DateTime.Now.AddHours(-1).AddMinutes(-2);
    }

    public void CalculateTimeDifference()
    {
        Driver.Navigate().GoToUrl(GameMainUrl);

        var now = DateTime.Now;
        var elements = Awaiter.Until(x => x.FindElements(By.XPath("//center/div/div")));

        var isDetected = elements.Any();

        if (isDetected)
        {
            foreach (var element in elements)
            {
                var text = element.Text;

                var isMatch = Regex.IsMatch(text, @"(\d{2}):(\d{2})");

                if (isMatch)
                {
                    var match = Regex.Match(text, @"(\d{2}):(\d{2})");

                    var hours = Convert.ToInt32(match.Groups[1].Value);
                    var minutes = Convert.ToInt32(match.Groups[2].Value);

                    HoursDifferance = now.Hour - hours;
                    MinutesDifferance = now.Minute - minutes;
                    break;
                }
            }
        }
    }
}
