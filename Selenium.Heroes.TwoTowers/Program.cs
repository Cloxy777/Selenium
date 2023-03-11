using Polly;
using Selenium.Heroes.Common.Configuration;
using Selenium.Heroes.TwoTowers;

HeroesConfiguration.ReadConfiguration();

//Build the policy
var retryPolicy = Policy.Handle<Exception>()
    .WaitAndRetry(retryCount: 300, sleepDurationProvider: _ => TimeSpan.FromSeconds(1));


//Execute the error prone code with the policy
var attempt = 0;
retryPolicy.Execute(() =>
{
    Console.WriteLine($"Global start. Attempt: {attempt++}.");
    Startup.Run();
});

