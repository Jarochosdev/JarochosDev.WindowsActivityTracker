using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Proxies;
using JarochosDev.WindowsActivityTracker.Common.Test.Fakes;
using JarochosDev.WindowsActivityTracker.Common.Test.Utilities;
using Microsoft.Win32;
using Moq;
using NuGet.Frameworks;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test
{
    class WindowsSystemEventListenerTest
    {
        private WindowsSystemEventListener _windowsSystemEventListener;
        private Mock<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>> _mockPowerModeToSystemEventConverter;
        private Mock<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>> _mockSessionEndedToSystemEventConverter;
        private Mock<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>> _mockSessionSwitchToSystemEventConverter;

        [SetUp]
        public void SetUp()
        {
            _mockPowerModeToSystemEventConverter = new Mock<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>>();
            _mockSessionEndedToSystemEventConverter = new Mock<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>>();
            _mockSessionSwitchToSystemEventConverter = new Mock<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>>();
            _windowsSystemEventListener = new WindowsSystemEventListener(_mockPowerModeToSystemEventConverter.Object,
                _mockSessionEndedToSystemEventConverter.Object,
                _mockSessionSwitchToSystemEventConverter.Object);
        }

        [Test]
        public void Test_Constructor()
        {
            var mockPowerModeToSystemEventConverter = new Mock<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>>();
            var mockSessionEndedToSystemEventConverter = new Mock<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>>();
            var mockSessionSwitchToSystemEventConverter = new Mock<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>>();
            var windowsSystemEventListener = new WindowsSystemEventListener(mockPowerModeToSystemEventConverter.Object,
                mockSessionEndedToSystemEventConverter.Object,
                mockSessionSwitchToSystemEventConverter.Object);

            Assert.AreSame(mockPowerModeToSystemEventConverter.Object, windowsSystemEventListener.PowerModeToSystemEventConverter);
            Assert.AreSame(mockSessionEndedToSystemEventConverter.Object, windowsSystemEventListener.SessionEndedToSystemEventConverter);
            Assert.AreSame(mockSessionSwitchToSystemEventConverter.Object, windowsSystemEventListener.SessionSwitchToSystemEventConverter);
        }

        [Test]
        public void Test_Subscribe_Add_ObserverToInternalList_Only_One_Time_And_Returns_Unsubscriber_WithTheInternalListAndTheObserver()
        {
            var mockObserver = new Mock<IObserver<IWindowsSystemEvent>>();

            var disposable = _windowsSystemEventListener.Subscribe(mockObserver.Object);

            Assert.AreEqual(1, _windowsSystemEventListener.Observers.Count);
            Assert.AreSame(mockObserver.Object, _windowsSystemEventListener.Observers.ElementAt(0));

            Assert.IsInstanceOf<UnsubscriberObserver<IWindowsSystemEvent>>(disposable);
            var unsubscriberObserver = disposable as UnsubscriberObserver<IWindowsSystemEvent>;

            CollectionAssert.AreEqual(_windowsSystemEventListener.Observers, unsubscriberObserver.ReadOnlyObservers);
            Assert.AreSame(mockObserver.Object, unsubscriberObserver.ObserverToUnsubscribe);

            _windowsSystemEventListener.Subscribe(mockObserver.Object);

            Assert.AreEqual(1, _windowsSystemEventListener.Observers.Count);
            Assert.AreSame(mockObserver.Object, _windowsSystemEventListener.Observers.ElementAt(0));

        }

        [Test, UseMockProxySystemEvents]
        public void Test_Start_Subscribe_Observers_To_ProxySystemEvent_Only_One_Time()
        {
            var mockProxySystemEvents = UnitTestUtilities.SingletonService.GetMockSingleton<ProxySystemEvents>() as Mock<IProxySystemEvents>;

            mockProxySystemEvents
                .Setup(e=>e.Subscribe(It.IsAny<IObserver<PowerModeChangedEventArgs>>()))
                .Callback<IObserver<PowerModeChangedEventArgs>>(observerPassed =>
                {
                    Assert.IsInstanceOf<PowerModeChangedObserver>(observerPassed);
                    var powerModeChangedObserver = observerPassed as PowerModeChangedObserver;
                    Assert.AreSame(_mockPowerModeToSystemEventConverter.Object, powerModeChangedObserver.PowerModeToSystemEventConverter);
                    Assert.AreSame(_windowsSystemEventListener, powerModeChangedObserver.WindowsSystemEventHandler);
                });

            mockProxySystemEvents
                .Setup(e => e.Subscribe(It.IsAny<IObserver<SessionSwitchEventArgs>>()))
                .Callback<IObserver<SessionSwitchEventArgs>>(observerPassed =>
                {
                    Assert.IsInstanceOf<SessionSwitchObserver>(observerPassed);
                    var sessionSwitchObserver = observerPassed as SessionSwitchObserver;
                    Assert.AreSame(_mockSessionSwitchToSystemEventConverter.Object, sessionSwitchObserver.SessionSwitchToSystemEventConverter);
                    Assert.AreSame(_windowsSystemEventListener, sessionSwitchObserver.WindowsSystemEventHandler);
                });

            mockProxySystemEvents
                .Setup(e => e.Subscribe(It.IsAny<IObserver<SessionEndedEventArgs>>()))
                .Callback<IObserver<SessionEndedEventArgs>>(observerPassed =>
                {
                    Assert.IsInstanceOf<SessionEndedObserver>(observerPassed);
                    var sessionEndedObserver = observerPassed as SessionEndedObserver;
                    Assert.AreSame(_mockSessionEndedToSystemEventConverter.Object, sessionEndedObserver.SessionEndedToSystemEventConverter);
                    Assert.AreSame(_windowsSystemEventListener, sessionEndedObserver.WindowsSystemEventHandler);
                });

            _windowsSystemEventListener.Start();
            _windowsSystemEventListener.Start();

            mockProxySystemEvents.VerifyAll();

            mockProxySystemEvents.Verify(e => e.Subscribe(It.IsAny<IObserver<PowerModeChangedEventArgs>>()), Times.Once);
            mockProxySystemEvents.Verify(e => e.Subscribe(It.IsAny<IObserver<SessionSwitchEventArgs>>()), Times.Once);
            mockProxySystemEvents.Verify(e => e.Subscribe(It.IsAny<IObserver<SessionEndedEventArgs>>()), Times.Once);
        }

        [Test, UseMockProxySystemEvents]
        public void Test_Stop_CallsToDisposeEach_Observer_Subscribed_ToProxySystemEvent_Only_One_Time()
        {
            var mockProxySystemEvents = UnitTestUtilities.SingletonService.GetMockSingleton<ProxySystemEvents>() as Mock<IProxySystemEvents>;

            var mockDisposable1 = new Mock<IDisposable>();
            var mockDisposable2 = new Mock<IDisposable>();
            var mockDisposable3 = new Mock<IDisposable>();

            mockProxySystemEvents
                .Setup(e => e.Subscribe(It.IsAny<IObserver<PowerModeChangedEventArgs>>()))
                .Returns(mockDisposable1.Object);

            mockProxySystemEvents
                .Setup(e => e.Subscribe(It.IsAny<IObserver<SessionSwitchEventArgs>>()))
                .Returns(mockDisposable2.Object);

            mockProxySystemEvents
                .Setup(e => e.Subscribe(It.IsAny<IObserver<SessionEndedEventArgs>>()))
                .Returns(mockDisposable3.Object);

            _windowsSystemEventListener.Start();
            _windowsSystemEventListener.Stop();
            _windowsSystemEventListener.Stop();

            mockDisposable1.Verify(d=>d.Dispose(), Times.Once);
            mockDisposable2.Verify(d=>d.Dispose(), Times.Once);
            mockDisposable3.Verify(d=>d.Dispose(), Times.Once);
        }

        [Test]
        public void Test_Handle_Calls_OnNext_ForEachObserver_Subscribed_To_WindowsSystemEventListener()
        {
            var fakeWindowsSystemEvent = new FakeWindowsSystemEvent();

            var mockObserver1 = new Mock<IObserver<IWindowsSystemEvent>>();
            var mockObserver2 = new Mock<IObserver<IWindowsSystemEvent>>();

            _windowsSystemEventListener.Subscribe(mockObserver1.Object);
            _windowsSystemEventListener.Subscribe(mockObserver2.Object);

            _windowsSystemEventListener.Handle(fakeWindowsSystemEvent);

            mockObserver1.Verify(o=>o.OnNext(fakeWindowsSystemEvent));
            mockObserver2.Verify(o=>o.OnNext(fakeWindowsSystemEvent));

        }
    }
}
