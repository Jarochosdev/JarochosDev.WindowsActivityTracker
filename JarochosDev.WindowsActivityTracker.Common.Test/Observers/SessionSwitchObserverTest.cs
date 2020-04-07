using System;
using System.Collections.Generic;
using System.Text;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Test.Fakes;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Observers
{
    class SessionSwitchObserverTest
    {
        private Mock<IWindowsSystemEventHandler> _mockWindowsSystemEventHandler;
        private Mock<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>> _mockConverter;
        private SessionSwitchObserver _observer;

        [SetUp]
        public void SetUp()
        {
            _mockWindowsSystemEventHandler = new Mock<IWindowsSystemEventHandler>();
            _mockConverter = new Mock<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>>();
            _observer = new SessionSwitchObserver(_mockWindowsSystemEventHandler.Object, _mockConverter.Object);
        }

        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(_mockWindowsSystemEventHandler.Object, _observer.WindowsSystemEventHandler);
            Assert.AreSame(_mockConverter.Object, _observer.SessionSwitchToSystemEventConverter);
        }

        [Test]
        public void Test_OnNext_Calls_EventHandler_And_Converter()
        {
            var windowsSystemEvent = new FakeWindowsSystemEvent();

            var mockToConvert = new Mock<SessionSwitchEventArgs>(null);
            _mockConverter.Setup(c => c.Convert(mockToConvert.Object)).Returns(windowsSystemEvent);
            _mockWindowsSystemEventHandler.Setup(h => h.Handle(windowsSystemEvent));

            _observer.OnNext(mockToConvert.Object);

            _mockWindowsSystemEventHandler.VerifyAll();
        }
    }
}
