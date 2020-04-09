using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Test.Fakes;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Observers
{
    class TextMessageEventLoggerObserverTest
    {
        [Test]
        public void Test_Constructor()
        {
            var mockMessageLogger = new Mock<IMessageLogger>();
            var textMessageEventLoggerObserver = new TextMessageEventLoggerObserver(mockMessageLogger.Object);
            Assert.AreSame(mockMessageLogger.Object, textMessageEventLoggerObserver.MessageLogger);
        }

        [Test]
        public void Test_OnNext_Calls_Logger_Log()
        {
            var mockMessageLogger = new Mock<IMessageLogger>();
            var textMessageEventLoggerObserver = new TextMessageEventLoggerObserver(mockMessageLogger.Object);

            var type= WindowsSystemEventType.None;
            var eventMessage = "Event Message";
            var dateTime = new DateTime(2019,09,08);
            var machineName = "Machine";
            var userName = "username";

            var fakeWindowsSystemEvent = new FakeWindowsSystemEvent()
            {
                Type = type,
                EventMessage = eventMessage,
                DateTime = dateTime,
                MachineName = machineName,
                UserName = userName
            };
            textMessageEventLoggerObserver.OnNext(fakeWindowsSystemEvent);

            var expectedMessage = $"Log Entry : " + type + $" {eventMessage} {userName} {machineName} " + dateTime;

            mockMessageLogger.Verify(l=>l.Log(expectedMessage));
        }
    }
}
