using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using NUnit.Framework;
using System.ServiceProcess;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using Moq;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Test.Converters
{
    class PowerModeChangedEventArgsToWindowsSystemEventConverterTest
    {
        [Test]
        [TestCase(PowerBroadcastStatus.BatteryLow, WindowsSystemEventType.None)]
        [TestCase(PowerBroadcastStatus.OemEvent, WindowsSystemEventType.None)]
        [TestCase(PowerBroadcastStatus.PowerStatusChange, WindowsSystemEventType.None)]
        [TestCase(PowerBroadcastStatus.QuerySuspend, WindowsSystemEventType.StopWorking)]
        [TestCase(PowerBroadcastStatus.Suspend, WindowsSystemEventType.StopWorking)]
        [TestCase(PowerBroadcastStatus.ResumeAutomatic, WindowsSystemEventType.StartWorking)]
        [TestCase(PowerBroadcastStatus.ResumeCritical, WindowsSystemEventType.StartWorking)]
        [TestCase(PowerBroadcastStatus.ResumeSuspend, WindowsSystemEventType.StartWorking)]
        [TestCase(PowerBroadcastStatus.QuerySuspendFailed, WindowsSystemEventType.StartWorking)]
        public void Test_Convert_Returns_WindowsSystemEvent(PowerBroadcastStatus powerBroadcastStatus, WindowsSystemEventType windowsSystemEventTypeExpected)
        {
            var mockWindowsSystemEventConstructor = new Mock<IWindowsSystemEventConstructor>();
            var powerBroadcastToWindowsSystemEvent = new PowerBroadcastToWindowsSystemEvent(mockWindowsSystemEventConstructor.Object);

            var message = $"PowerBroadcastEvent:{powerBroadcastStatus}";

            var mockWindowsSystemEvent = new Mock<IWindowsSystemEvent>();
            mockWindowsSystemEventConstructor
                .Setup(c => c.Construct(message, windowsSystemEventTypeExpected))
                .Returns(mockWindowsSystemEvent.Object);

            var converted = powerBroadcastToWindowsSystemEvent.Convert(powerBroadcastStatus);

            Assert.AreSame(mockWindowsSystemEvent.Object, converted);
        }
    }
}
