// add configuration
// add to git
// add DI
// add check for free places
// remove not used arguments
// add options
// save image crossplatform


using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Configuration;
using Selenium.Heroes.Worker;

var useCaptchaControl = Environment.GetCommandLineArgs().Any(x => x == "-useCaptchaControl");

HeroesWorkerEngine.UseCaptchaControl = useCaptchaControl;

Windows.SetLongRunningConsoleMode();

HeroesConfiguration.ReadConfiguration();

Startup.Run();
