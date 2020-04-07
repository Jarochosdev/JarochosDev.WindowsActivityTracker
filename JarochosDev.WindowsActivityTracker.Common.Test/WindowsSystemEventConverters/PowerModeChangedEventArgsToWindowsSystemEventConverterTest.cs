using System;
using System.Collections.Generic;
using System.Text;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Win32;
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
            var powerModeChangedEventArgsToWindowsSystemEventConverter = new PowerModeChangedEventArgsToWindowsSystemEventConverter();

            var converted = powerModeChangedEventArgsToWindowsSystemEventConverter.Convert(new PowerModeChangedEventArgs(powerMode));

            Assert.AreEqual(windowsSystemEventTypeExpected, converted.Type);
            Assert.AreEqual($"PowerModeChangedEvent:{powerMode}", converted.EventMessage);
        }
    }
}
