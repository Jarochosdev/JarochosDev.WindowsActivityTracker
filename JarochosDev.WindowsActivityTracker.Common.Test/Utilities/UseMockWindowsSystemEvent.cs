using System;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.WindowsActivityTracker.Common.Proxies;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Utilities
{
    public class UseMockProxySystemEvents : Attribute, ITestAction
    {

        public void BeforeTest(ITest test)
        {
            UnitTestUtilities.SingletonService.CreateMockSingleton<ProxySystemEvents, IProxySystemEvents>();
        }

        public void AfterTest(ITest test)
        {
            UnitTestUtilities.SingletonService.ClearMockSingleton<ProxySystemEvents, IProxySystemEvents>();
        }

        public ActionTargets Targets { get; }
    }
} 