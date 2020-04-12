using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Services;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dependencyInjectionModules = new List<IServiceModule>(){new DiCoreServiceModule(new EnvironmentDatabaseConfiguration())};
            ServiceRunner.Instance().RunEndless(new ConsoleWindowsActivityTrackerService(dependencyInjectionModules));
        }
    }
}
