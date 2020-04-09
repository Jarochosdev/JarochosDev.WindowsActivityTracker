using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using NUnit.Framework;
using System.ServiceProcess;

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
            var powerBroadcastToWindowsSystemEvent = new PowerBroadcastToWindowsSystemEvent();

            var converted = powerBroadcastToWindowsSystemEvent.Convert(powerBroadcastStatus);

            Assert.AreEqual(windowsSystemEventTypeExpected, converted.Type);
            Assert.AreEqual($"PowerBroadcastEvent:{powerBroadcastStatus}", converted.EventMessage);
        }
    }
}
