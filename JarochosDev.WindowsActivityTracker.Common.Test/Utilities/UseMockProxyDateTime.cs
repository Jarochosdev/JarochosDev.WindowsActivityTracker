using System;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxies;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Utilities
{
    public class UseMockProxyDateTime : Attribute, ITestAction
    {

        public void BeforeTest(ITest test)
        {
            UnitTestUtilities.SingletonService.CreateMockSingleton<ProxyDateTime, IProxyDateTime>();
        }

        public void AfterTest(ITest test)
        {
            UnitTestUtilities.SingletonService.ClearMockSingleton<ProxyDateTime, IProxyDateTime>();
        }

        public ActionTargets Targets { get; }
    }
}