using System.Linq;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.Processes;
using JarochosDev.Utilities.Net.NetStandard.Common.Services;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Console.Test.Utilities;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Console.Test
{
    public class ProgramTest
    {

        [Test]
        [UseMockConsoleServiceRunner]
        public void Test_Program_Calls_ConsoleServiceRunner_RunEndless()
        {
            var mockConsoleServiceRunner = UnitTestUtilities.SingletonService.GetMockSingleton<ServiceRunner>() as Mock<IServiceRunner>;
            
            mockConsoleServiceRunner
                .Setup(s => s.RunEndless(It.IsAny<IStartableProcess>()))
                .Callback<IStartableProcess>(servicePassed =>
                {
                    Assert.IsInstanceOf(typeof(ConsoleWindowsActivityTrackerService), servicePassed);
                    var consoleWindowsActivityTrackerService = servicePassed as ConsoleWindowsActivityTrackerService;

                    Assert.AreEqual(1, consoleWindowsActivityTrackerService.ServiceModules.Count);
                    var serviceModule = consoleWindowsActivityTrackerService.ServiceModules.ElementAt(0);
                    Assert.IsInstanceOf(typeof(DiCoreServiceModule), serviceModule);
                    var diCoreServiceModule = serviceModule as DiCoreServiceModule;
                    Assert.IsInstanceOf<EnvironmentDatabaseConfiguration>(diCoreServiceModule.DatabaseConnectionStringConfiguration);
                });

            Program.Main(null);

            mockConsoleServiceRunner.VerifyAll();
        }
    }
}
