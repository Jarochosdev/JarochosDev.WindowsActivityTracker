using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.WindowsSystemEventConverters
{
    class SessionSwitchEventArgsToWindowsSystemEventConverterTest
    {
        [Test]
        [TestCase(SessionSwitchReason.ConsoleDisconnect, WindowsSystemEventType.StopWorking)]
        [TestCase(SessionSwitchReason.RemoteDisconnect, WindowsSystemEventType.StopWorking)]
        [TestCase(SessionSwitchReason.SessionLock, WindowsSystemEventType.StopWorking)]
        [TestCase(SessionSwitchReason.SessionLogoff, WindowsSystemEventType.StopWorking)]
        [TestCase(SessionSwitchReason.ConsoleConnect, WindowsSystemEventType.StartWorking)]
        [TestCase(SessionSwitchReason.RemoteConnect, WindowsSystemEventType.StartWorking)]
        [TestCase(SessionSwitchReason.SessionLogon, WindowsSystemEventType.StartWorking)]
        [TestCase(SessionSwitchReason.SessionUnlock, WindowsSystemEventType.StartWorking)]
        [TestCase(SessionSwitchReason.SessionRemoteControl, WindowsSystemEventType.StartWorking)]
        public void Test_Convert_Returns_WindowsSystemEvent(SessionSwitchReason reason, WindowsSystemEventType windowsSystemEventTypeExpected)
        {
            var mockWindowsSystemEventConstructor = new Mock<IWindowsSystemEventConstructor>();
            var sessionSwitchEventArgsToWindowsSystemEventConverter = new SessionSwitchEventArgsToWindowsSystemEventConverter(mockWindowsSystemEventConstructor.Object);

            var mockWindowsSystemEvent = new Mock<IWindowsSystemEvent>();

            mockWindowsSystemEventConstructor
                .Setup(c => c.Construct($"SessionSwitchEvent:{reason}", windowsSystemEventTypeExpected))
                .Returns(mockWindowsSystemEvent.Object);

            var converted = sessionSwitchEventArgsToWindowsSystemEventConverter.Convert(new SessionSwitchEventArgs(reason));

            Assert.AreEqual(mockWindowsSystemEvent.Object, converted);
        }
    }
}
