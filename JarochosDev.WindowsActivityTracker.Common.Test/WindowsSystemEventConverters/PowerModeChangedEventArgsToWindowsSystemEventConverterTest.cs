using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.WindowsSystemEventConverters
{
    class PowerModeChangedEventArgsToWindowsSystemEventConverterTest
    {
        [Test]
        [TestCase(PowerModes.Resume, WindowsSystemEventType.StartWorking)]
        [TestCase(PowerModes.Suspend, WindowsSystemEventType.StopWorking)]
        [TestCase(PowerModes.StatusChange, WindowsSystemEventType.None)]
        public void Test_Convert_Returns_WindowsSystemEvent(PowerModes powerMode, WindowsSystemEventType windowsSystemEventTypeExpected)
        {
            var mockWindowsSystemEventConstructor = new Mock<IWindowsSystemEventConstructor>();
            var powerModeChangedEventArgsToWindowsSystemEventConverter = new PowerModeChangedEventArgsToWindowsSystemEventConverter(mockWindowsSystemEventConstructor.Object);

            var mockWindowsSystemEvent = new Mock<IWindowsSystemEvent>();

            mockWindowsSystemEventConstructor
                .Setup(c => c.Construct($"PowerModeChangedEvent:{powerMode}", windowsSystemEventTypeExpected))
                .Returns(mockWindowsSystemEvent.Object);

            var converted = powerModeChangedEventArgsToWindowsSystemEventConverter.Convert(new PowerModeChangedEventArgs(powerMode));

            Assert.AreEqual(mockWindowsSystemEvent.Object, converted);
        }
    }
}
