﻿using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Test.Fakes;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Observers
{
    class PowerModeChangedObserverTest
    {
        private Mock<IWindowsSystemEventHandler> _mockWindowsSystemEventHandler;
        private Mock<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>> _mockConverter;
        private PowerModeChangedObserver _observer;

        [SetUp]
        public void SetUp()
        {
            _mockWindowsSystemEventHandler = new Mock<IWindowsSystemEventHandler>();
            _mockConverter = new Mock<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>>();
            _observer = new PowerModeChangedObserver(_mockWindowsSystemEventHandler.Object, _mockConverter.Object);
        }

        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(_mockWindowsSystemEventHandler.Object, _observer.WindowsSystemEventHandler);
            Assert.AreSame(_mockConverter.Object, _observer.PowerModeToSystemEventConverter);
        }

        [Test]
        public void Test_OnNext_Calls_EventHandler_And_Converter()
        {
            var windowsSystemEvent = new FakeWindowsSystemEvent();

            var mockToConvert = new Mock<PowerModeChangedEventArgs>(null);
            _mockConverter.Setup(c => c.Convert(mockToConvert.Object)).Returns(windowsSystemEvent);
            _mockWindowsSystemEventHandler.Setup(h => h.Handle(windowsSystemEvent));

            _observer.OnNext(mockToConvert.Object);

            _mockWindowsSystemEventHandler.VerifyAll();
        }
    }
}
