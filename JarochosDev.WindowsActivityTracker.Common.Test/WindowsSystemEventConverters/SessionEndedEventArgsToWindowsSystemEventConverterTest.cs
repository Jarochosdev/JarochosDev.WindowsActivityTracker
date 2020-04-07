using System;
using System.Collections.Generic;
using System.Text;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Win32;
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
            var sessionEndedEventArgsToWindowsSystemEventConverter = new SessionEndedEventArgsToWindowsSystemEventConverter();

            var converted = sessionEndedEventArgsToWindowsSystemEventConverter.Convert(new SessionEndedEventArgs(reason));

            Assert.AreEqual(windowsSystemEventTypeExpected, converted.Type);
            Assert.AreEqual($"SessionEndedEvent:{reason}", converted.EventMessage);
        }
    }
}
