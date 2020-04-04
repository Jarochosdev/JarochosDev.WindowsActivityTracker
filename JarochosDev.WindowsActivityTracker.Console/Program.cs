using System.Collections.Generic;
using JarochosDev.Utilities.NetStandard.ConsoleApp;
using JarochosDev.Utilities.NetStandard.ConsoleApp.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dependencyInjectionModules = new List<IServiceModule>(){new DiServiceModule()};
            ConsoleServiceRunner.Instance().RunEndless(new ConsoleWindowsActivityTrackerService(dependencyInjectionModules));
        }
    }
}
