using System;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxies;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Test.Utilities;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Models
{
    class WindowsSystemEventTest
    {
        [Test, UseMockProxyDateTime, UseMockProxyEnvironment]
        public void Test_Constructor()
        {
            var mockProxyDateTime = UnitTestUtilities.SingletonService.GetMockSingleton<ProxyDateTime>() as Mock<IProxyDateTime>;
            var mockProxyEnvironment = UnitTestUtilities.SingletonService.GetMockSingleton<ProxyEnvironment>() as Mock<IProxyEnvironment>;

            var dateTime = new DateTime();
            mockProxyDateTime.Setup(d => d.Now()).Returns(dateTime);

            string environmentUserName = "user name";
            string environmentMachineName = "machine name";

            mockProxyEnvironment.Setup(e => e.UserName).Returns(environmentUserName);
            mockProxyEnvironment.Setup(e => e.MachineName).Returns(environmentMachineName);

            var eventMessage = "eventMessage";
            var windowsSystemEventType = WindowsSystemEventType.None;
            var windowsSystemEvent = new WindowsSystemEvent(eventMessage, windowsSystemEventType);
            Assert.AreEqual(eventMessage, windowsSystemEvent.EventMessage);
            Assert.AreEqual(windowsSystemEventType, windowsSystemEvent.Type);
            Assert.AreEqual(dateTime, windowsSystemEvent.DateTime);
            Assert.AreEqual(environmentUserName, windowsSystemEvent.UserName);
            Assert.AreEqual(environmentMachineName, windowsSystemEvent.MachineName);
        }
    }
}
