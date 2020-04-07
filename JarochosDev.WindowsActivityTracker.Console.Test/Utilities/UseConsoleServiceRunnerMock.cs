using System;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace JarochosDev.WindowsActivityTracker.Console.Test.Utilities
{
    public class UseConsoleServiceRunnerMock : Attribute, ITestAction
    {

        public void BeforeTest(ITest test)
        {
            UnitTestUtilities.SingletonService.CreateMockSingleton<ConsoleServiceRunner, IConsoleServiceRunner>();
        }

        public void AfterTest(ITest test)
        {
            UnitTestUtilities.SingletonService.ClearMockSingleton<ConsoleServiceRunner, IConsoleServiceRunner>();
        }

        public ActionTargets Targets { get; }
    }
}