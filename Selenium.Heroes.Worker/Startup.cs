using Microsoft.Extensions.Configuration;

namespace Selenium.Heroes.Worker;

public class Startup
{
    public static void Run() 
    {
        var worker = new HeroesWorkerEngine();
        worker.Authenticate();
        worker.CalculateTimeDifference();

        while (true)
        {
            var lastWorkTime = worker.LastWorkTime;
            var nextWorkTime = lastWorkTime.AddHours(1).AddMinutes(1);
            var difference = nextWorkTime.Subtract(DateTime.Now);

            var hours = difference.Hours > 0 ? difference.Hours : 0;
            var minutes = difference.Minutes > 0 ? difference.Minutes : 0;
            var seconds = difference.Seconds > 0 ? difference.Seconds: 0;

            var left = hours * 60 * 60 * 1000 + minutes * 60 * 1000 + seconds * 1000;
            if (left > 0)
            {
                Console.WriteLine($"Work in {hours}h {minutes}m {seconds}s...");
                Thread.Sleep(5 * 1000);
                continue;
            }

            try
            {
                worker.StartWork();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Global exception handler worked.");
            }
        }
    } 
}
