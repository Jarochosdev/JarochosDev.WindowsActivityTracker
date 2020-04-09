using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.WindowsSystemEventConverters
{
    class SessionEndedEventArgsToWindowsSystemEventConverterTest
    {
        [Test]
        [TestCase(SessionEndReasons.Logoff, WindowsSystemEventType.StopWorking)]
        [TestCase(SessionEndReasons.SystemShutdown, WindowsSystemEventType.StopWorking)]
        public void Test_Convert_Returns_WindowsSystemEvent(SessionEndReasons reason, WindowsSystemEventType windowsSystemEventTypeExpected)
        {
            var mockWindowsSystemEventConstructor = new Mock<IWindowsSystemEventConstructor>();
            var sessionEndedEventArgsToWindowsSystemEventConverter = new SessionEndedEventArgsToWindowsSystemEventConverter(mockWindowsSystemEventConstructor.Object);

            var mockWindowsSystemEvent = new Mock<IWindowsSystemEvent>();

            mockWindowsSystemEventConstructor
                .Setup(c => c.Construct($"SessionEndedEvent:{reason}", windowsSystemEventTypeExpected))
                .Returns(mockWindowsSystemEvent.Object);

            var converted = sessionEndedEventArgsToWindowsSystemEventConverter.Convert(new SessionEndedEventArgs(reason));

            Assert.AreEqual(mockWindowsSystemEvent.Object, converted);
        }
    }
}
