using System;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.Services;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace JarochosDev.WindowsActivityTracker.Console.Test.Utilities
{
    public class UseMockConsoleServiceRunner : Attribute, ITestAction
    {

        public void BeforeTest(ITest test)
        {
            UnitTestUtilities.SingletonService.CreateMockSingleton<ServiceRunner, IServiceRunner>();
        }

        public void AfterTest(ITest test)
        {
            UnitTestUtilities.SingletonService.ClearMockSingleton<ServiceRunner, IServiceRunner>();
        }

        public ActionTargets Targets { get; }
    }
}