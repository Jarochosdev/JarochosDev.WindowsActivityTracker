using System;
using System.Collections.Generic;
using JarochosDev.TestUtilities.NetStandard;
using JarochosDev.Utilities.NetStandard.ConsoleApp;
using JarochosDev.Utilities.NetStandard.ConsoleApp.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common;
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
        public void Test_StartService_SubscribeDatabaseEventLogger_To_WindowsServiceListener_And_Start()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockWindowsSystemEventListener = new Mock<IWindowsSystemEventListener>();
            var mockDatabaseSystemEventLogger = new Mock<DatabaseSystemEventObserver>(null);
            
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsSystemEventListener)))
                .Returns(mockWindowsSystemEventListener.Object);

            mockServiceProvider
                .Setup(p => p.GetService(typeof(DatabaseSystemEventObserver)))
                .Returns(mockDatabaseSystemEventLogger.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>(_consoleWindowsActivityTrackerService, "StartService", mockServiceProvider.Object);

            mockWindowsSystemEventListener.Verify(e=>e.Subscribe(mockDatabaseSystemEventLogger.Object), Times.Once);
            mockWindowsSystemEventListener.Verify(e=>e.Start(), Times.Once);
            mockWindowsSystemEventListener.Verify(l => l.Stop(), Times.Never);
        }

        [Test]
        public void Test_StopService_Dispose_SubscribedEventToEventListener_And_Stop()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockWindowsSystemEventListener = new Mock<IWindowsSystemEventListener>();
            var mockDatabaseSystemEventLogger = new Mock<DatabaseSystemEventObserver>(null);

            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsSystemEventListener)))
                .Returns(mockWindowsSystemEventListener.Object);

            mockServiceProvider
                .Setup(p => p.GetService(typeof(DatabaseSystemEventObserver)))
                .Returns(mockDatabaseSystemEventLogger.Object);

            var mockSubscribedItem = new Mock<IDisposable>();
            mockWindowsSystemEventListener.Setup(l => l.Subscribe(mockDatabaseSystemEventLogger.Object))
                .Returns(mockSubscribedItem.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>(_consoleWindowsActivityTrackerService, "StartService", mockServiceProvider.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>(_consoleWindowsActivityTrackerService, "StopService", mockServiceProvider.Object);

            mockSubscribedItem.Verify(s=>s.Dispose());
            mockWindowsSystemEventListener.Verify(l=>l.Stop(), Times.Once);
        }
    }
}
