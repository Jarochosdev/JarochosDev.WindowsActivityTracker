using System;
using System.Collections.Generic;
using System.ServiceProcess;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Test
{
    class WindowsActivityTrackerServiceTest
    {
        private Mock<IServiceModule> _mockWindowsServiceModule;
        private Mock<IServiceProviderBuilder> _mockServiceProviderBuilder;
        private Mock<IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent>> _mockPowerBroadcastToWindowsSystemEvent;
        private Mock<IObjectConverter<SessionChangeDescription, IWindowsSystemEvent>> _mockSessionChangeToWindowsSystemEvent;
        private List<IObserver<IWindowsSystemEvent>> _windowsSystemEventObservers;
        private WindowsActivityTrackerService _windowsActivityTrackerService;
        private Mock<IObserver<IWindowsSystemEvent>> _mockObserver1;
        private Mock<IObserver<IWindowsSystemEvent>> _mockObserver2;

        [SetUp]
        public void SetUp()
        {
            _mockWindowsServiceModule = new Mock<IServiceModule>();
            _mockServiceProviderBuilder = new Mock<IServiceProviderBuilder>();
            _mockPowerBroadcastToWindowsSystemEvent = new Mock<IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent>>();
            _mockSessionChangeToWindowsSystemEvent = new Mock<IObjectConverter<SessionChangeDescription, IWindowsSystemEvent>>();
            _mockObserver1 = new Mock<IObserver<IWindowsSystemEvent>>();
            _mockObserver2 = new Mock<IObserver<IWindowsSystemEvent>>();
            _windowsSystemEventObservers = new List<IObserver<IWindowsSystemEvent>>() {_mockObserver1.Object, _mockObserver2.Object};
            _windowsActivityTrackerService = new WindowsActivityTrackerService(_mockWindowsServiceModule.Object,
                _mockServiceProviderBuilder.Object,
                _mockPowerBroadcastToWindowsSystemEvent.Object,
                _mockSessionChangeToWindowsSystemEvent.Object,
                _windowsSystemEventObservers);
        }

        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(_mockWindowsServiceModule.Object, _windowsActivityTrackerService.WindowsServiceModule);
            Assert.AreSame(_mockServiceProviderBuilder.Object, _windowsActivityTrackerService.ServiceProviderBuilder);
            Assert.AreSame(_mockPowerBroadcastToWindowsSystemEvent.Object, _windowsActivityTrackerService.PowerBroadcastToWindowsSystemEvent);
            Assert.AreSame(_mockSessionChangeToWindowsSystemEvent.Object, _windowsActivityTrackerService.SessionChangeToWindowsSystemEvent);
            Assert.AreSame(_windowsSystemEventObservers, _windowsActivityTrackerService.WindowsSystemEventObservers);

            Assert.AreEqual(AppConstants.LOGGING_APPLICATION_NAME, _windowsActivityTrackerService.ServiceName);
            Assert.AreEqual(true, _windowsActivityTrackerService.CanHandlePowerEvent);
            Assert.AreEqual(true, _windowsActivityTrackerService.CanHandleSessionChangeEvent);
            Assert.AreEqual(true, _windowsActivityTrackerService.CanShutdown);
            Assert.AreEqual(false, _windowsActivityTrackerService.AutoLog);
        }

        [Test]
        public void Test_OnStart_Create_ServiceProvider_Get_ThreadApplication_And_Started()
        {
            _mockServiceProviderBuilder
                .Setup(b => b.AddServiceModule(_mockWindowsServiceModule.Object))
                .Returns(_mockServiceProviderBuilder.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();
            _mockServiceProviderBuilder.Setup(b => b.Build()).Returns(mockServiceProvider.Object);

            var mockThreadFormApplicationRunner = new Mock<IThreadFormApplicationRunnerProcess>();
            
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IThreadFormApplicationRunnerProcess)))
                .Returns(mockThreadFormApplicationRunner.Object);

            UnitTestUtilities.Helper.RunProtectedMethod(_windowsActivityTrackerService, "OnStart");
            mockThreadFormApplicationRunner.Verify(r=>r.Start(), Times.Once);
            mockThreadFormApplicationRunner.Verify(r=>r.Stop(), Times.Never);
        }

        [Test]
        public void Test_OnStop_Calls_ToStop_ThreadApplicationRequestedTo_ServiceProvider_InStartCall()
        {
            _mockServiceProviderBuilder
                .Setup(b => b.AddServiceModule(_mockWindowsServiceModule.Object))
                .Returns(_mockServiceProviderBuilder.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();
            _mockServiceProviderBuilder.Setup(b => b.Build()).Returns(mockServiceProvider.Object);

            var mockThreadFormApplicationRunner = new Mock<IThreadFormApplicationRunnerProcess>();

            mockServiceProvider
                .Setup(p => p.GetService(typeof(IThreadFormApplicationRunnerProcess)))
                .Returns(mockThreadFormApplicationRunner.Object);

            UnitTestUtilities.Helper.RunProtectedMethod(_windowsActivityTrackerService, "OnStart");
            mockThreadFormApplicationRunner.Reset();

            UnitTestUtilities.Helper.RunProtectedMethod(_windowsActivityTrackerService, "OnStop");
            mockThreadFormApplicationRunner.Verify(r=>r.Stop(), Times.Once);
            mockThreadFormApplicationRunner.Verify(r=>r.Start(), Times.Never);
        }

        [Test]
        public void Test_OnPowerEvent_ConvertArgument_And_NotifyObservers()
        {
            var sessionChangeDescription = new SessionChangeDescription();
            var mockWindowsSystemEvent = new Mock<IWindowsSystemEvent>();

            _mockSessionChangeToWindowsSystemEvent
                .Setup(s => s.Convert(sessionChangeDescription))
                .Returns(mockWindowsSystemEvent.Object);

            UnitTestUtilities.Helper.RunProtectedMethod(
                _windowsActivityTrackerService,
                "OnSessionChange",
                new object[] {sessionChangeDescription});

            _mockObserver1.Verify(o=>o.OnNext(mockWindowsSystemEvent.Object));
            _mockObserver2.Verify(o=>o.OnNext(mockWindowsSystemEvent.Object));
        }

        [Test]
        public void Test_OnSessionChangeDescription_ConvertArgument_And_NotifyObservers()
        {
            var powerBroadcastStatus = new PowerBroadcastStatus();
            var mockWindowsSystemEvent = new Mock<IWindowsSystemEvent>();

            _mockPowerBroadcastToWindowsSystemEvent
                .Setup(s => s.Convert(powerBroadcastStatus))
                .Returns(mockWindowsSystemEvent.Object);

            UnitTestUtilities.Helper.RunProtectedMethod(
                _windowsActivityTrackerService,
                "OnPowerEvent",
                new object[] { powerBroadcastStatus });

            _mockObserver1.Verify(o => o.OnNext(mockWindowsSystemEvent.Object));
            _mockObserver2.Verify(o => o.OnNext(mockWindowsSystemEvent.Object));
        }

        [Test]
        public void Test_OnShutdown_ConvertArgument_And_NotifyObservers()
        {
            IWindowsSystemEvent windowsSystemEventPassed1 = null;
            IWindowsSystemEvent windowsSystemEventPassed2 = null;

            _mockObserver1
                .Setup(o => o.OnNext(It.IsAny<IWindowsSystemEvent>()))
                .Callback<IWindowsSystemEvent>(systemEventPassed => { windowsSystemEventPassed1 = systemEventPassed; });

            _mockObserver2
                .Setup(o => o.OnNext(It.IsAny<IWindowsSystemEvent>()))
                .Callback<IWindowsSystemEvent>(systemEventPassed => { windowsSystemEventPassed2 = systemEventPassed; });

            UnitTestUtilities.Helper.RunProtectedMethod(
                _windowsActivityTrackerService,
                "OnShutdown");

            Assert.IsNotNull(windowsSystemEventPassed1);
            Assert.AreSame(windowsSystemEventPassed1, windowsSystemEventPassed2);

            Assert.AreEqual("ShutdownEvent:None", windowsSystemEventPassed1.EventMessage);
            Assert.AreEqual(WindowsSystemEventType.StopWorking, windowsSystemEventPassed1.Type);
            
            _mockObserver1.VerifyAll();
            _mockObserver2.VerifyAll();
            
        }
    }
}
