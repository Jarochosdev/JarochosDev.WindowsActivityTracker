using System;
using System.Collections.Generic;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test
{
    class WindowsSystemEventServiceManagerTest
    {
        private Mock<IWindowsSystemEventListener> _mockWindowsSystemEventListener;
        private List<IObserver<IWindowsSystemEvent>> _observers;
        private WindowsSystemEventServiceManager _windowsSystemEventServiceManager;
        private Mock<IObserver<IWindowsSystemEvent>> _mockObserver1;
        private Mock<IObserver<IWindowsSystemEvent>> _mockObserver2;

        [SetUp]
        public void SetUp()
        {
            _mockObserver1 = new Mock<IObserver<IWindowsSystemEvent>>();
            _mockObserver2 = new Mock<IObserver<IWindowsSystemEvent>>();

            _mockWindowsSystemEventListener = new Mock<IWindowsSystemEventListener>();
            _observers = new List<IObserver<IWindowsSystemEvent>>() {_mockObserver1.Object, _mockObserver2.Object};

            _windowsSystemEventServiceManager = new WindowsSystemEventServiceManager(_mockWindowsSystemEventListener.Object, _observers);
        }

        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(_mockWindowsSystemEventListener.Object, _windowsSystemEventServiceManager.WindowsSystemEventListener);
            Assert.AreSame(_observers, _windowsSystemEventServiceManager.Observers);
        }

        [Test]
        public void Test_Start_SubscribeObserver_And_StartListener_Only_OneTime()
        {
            _windowsSystemEventServiceManager.Start();
            _windowsSystemEventServiceManager.Start();

            _mockWindowsSystemEventListener.Verify(l=>l.Subscribe(_mockObserver1.Object), Times.Once);
            _mockWindowsSystemEventListener.Verify(l=>l.Subscribe(_mockObserver2.Object), Times.Once);
            _mockWindowsSystemEventListener.Verify(l=>l.Start(), Times.Once);
        }

        [Test]
        public void Test_Stop_Dispose_Subscribed_Items_InStart_And_StopListener_Only_OneTime()
        {
            var mockDisposable1 = new Mock<IDisposable>();
            var mockDisposable2 = new Mock<IDisposable>();

            _mockWindowsSystemEventListener.Setup(l => l.Subscribe(_mockObserver1.Object)).Returns(mockDisposable1.Object);
            _mockWindowsSystemEventListener.Setup(l => l.Subscribe(_mockObserver2.Object)).Returns(mockDisposable2.Object);

            _windowsSystemEventServiceManager.Start();
            
            _mockWindowsSystemEventListener.Reset();

            _windowsSystemEventServiceManager.Stop();
            _windowsSystemEventServiceManager.Stop();

            mockDisposable1.Verify(d=>d.Dispose(), Times.Once);
            mockDisposable2.Verify(d=>d.Dispose(), Times.Once);
            _mockWindowsSystemEventListener.Verify(l=>l.Stop(), Times.Once);
        }
    }
}
