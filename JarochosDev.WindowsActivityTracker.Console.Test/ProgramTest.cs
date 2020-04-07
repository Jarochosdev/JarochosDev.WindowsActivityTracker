using System.Linq;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp;
using JarochosDev.WindowsActivityTracker.Console.Test.Utilities;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Console.Test
{
    public class ProgramTest
    {

        [Test]
        [UseConsoleServiceRunnerMock]
        public void Test_Program_Calls_ConsoleServiceRunner_RunEndless()
        {
            var mockConsoleServiceRunner = UnitTestUtilities.SingletonService.GetMockSingleton<ConsoleServiceRunner>() as Mock<IConsoleServiceRunner>;
            
            mockConsoleServiceRunner
                .Setup(s => s.RunEndless(It.IsAny<IConsoleService>()))
                .Callback<IConsoleService>(consoleServicePassed =>
                {
                    Assert.IsInstanceOf(typeof(ConsoleWindowsActivityTrackerService), consoleServicePassed);
                    var consoleWindowsActivityTrackerService = consoleServicePassed as ConsoleWindowsActivityTrackerService;

                    Assert.AreEqual(1, consoleWindowsActivityTrackerService.ServiceModules.Count);
                    Assert.IsInstanceOf(typeof(DiServiceModule), consoleWindowsActivityTrackerService.ServiceModules.ElementAt(0));
                });

            Program.Main(null);

            mockConsoleServiceRunner.VerifyAll();
        }
    }
}
