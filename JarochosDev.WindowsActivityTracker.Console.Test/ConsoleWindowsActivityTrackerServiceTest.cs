using System;
using System.Collections.Generic;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Console.Test
{
    class ConsoleWindowsActivityTrackerServiceTest
    {
        private List<IServiceModule> _dependencyInjectionModules;
        private ConsoleWindowsActivityTrackerService _consoleWindowsActivityTrackerService;

        [SetUp]
        public void SetUp()
        {
            _dependencyInjectionModules = new List<IServiceModule>(){new Mock<IServiceModule>().Object};
            _consoleWindowsActivityTrackerService = new ConsoleWindowsActivityTrackerService(_dependencyInjectionModules);
        }

        [Test]
        public void Test_Implements()
        {
            Assert.IsInstanceOf(typeof(AbstractConsoleService), _consoleWindowsActivityTrackerService);
        }

        [Test]
        public void Test_Constructor_Parameters()
        {
            CollectionAssert.AreEqual(_dependencyInjectionModules, _consoleWindowsActivityTrackerService.ServiceModules);
        }

        [Test]
        public void Test_StartService_SubscribeAllObserversInServiceProvider_To_WindowsServiceListener_And_Start()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockWindowsSystemEventListener = new Mock<IWindowsSystemEventListener>();

            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsSystemEventListener)))
                .Returns(mockWindowsSystemEventListener.Object);

            var mockObserverWindowsSystemEvent1 = new Mock<IObserver<IWindowsSystemEvent>>();
            var mockObserverWindowsSystemEvent2 = new Mock<IObserver<IWindowsSystemEvent>>();

            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                mockObserverWindowsSystemEvent1.Object,
                mockObserverWindowsSystemEvent2.Object
            };
            
            mockServiceProvider
                .Setup(p => p.GetService(typeof(List<IObserver<IWindowsSystemEvent>>)))
                .Returns(observers);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>
                    (_consoleWindowsActivityTrackerService, "StartService", mockServiceProvider.Object);

            mockWindowsSystemEventListener.Verify(e=>e.Subscribe(mockObserverWindowsSystemEvent1.Object), Times.Once);
            mockWindowsSystemEventListener.Verify(e=>e.Subscribe(mockObserverWindowsSystemEvent2.Object), Times.Once);
            mockWindowsSystemEventListener.Verify(e=>e.Start(), Times.Once);
            mockWindowsSystemEventListener.Verify(l => l.Stop(), Times.Never);
        }

        [Test]
        public void Test_StopService_Dispose_SubscribedObservers_And_StopListener()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockWindowsSystemEventListener = new Mock<IWindowsSystemEventListener>();

            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsSystemEventListener)))
                .Returns(mockWindowsSystemEventListener.Object);


            var mockObserverWindowsSystemEvent1 = new Mock<IObserver<IWindowsSystemEvent>>();
            var mockObserverWindowsSystemEvent2 = new Mock<IObserver<IWindowsSystemEvent>>();

            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                mockObserverWindowsSystemEvent1.Object,
                mockObserverWindowsSystemEvent2.Object
            };

            mockServiceProvider
                .Setup(p => p.GetService(typeof(List<IObserver<IWindowsSystemEvent>>)))
                .Returns(observers);

            var mockSubscribedItem1 = new Mock<IDisposable>();
            var mockSubscribedItem2 = new Mock<IDisposable>();
            
            mockWindowsSystemEventListener.Setup(l => l.Subscribe(mockObserverWindowsSystemEvent1.Object))
                .Returns(mockSubscribedItem1.Object);

            mockWindowsSystemEventListener.Setup(l => l.Subscribe(mockObserverWindowsSystemEvent2.Object))
                .Returns(mockSubscribedItem2.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>(_consoleWindowsActivityTrackerService, "StartService", mockServiceProvider.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>(_consoleWindowsActivityTrackerService, "StopService", mockServiceProvider.Object);

            mockSubscribedItem1.Verify(s=>s.Dispose());
            mockSubscribedItem2.Verify(s=>s.Dispose());

            mockWindowsSystemEventListener.Verify(l=>l.Stop(), Times.Once);
        }
    }
}
