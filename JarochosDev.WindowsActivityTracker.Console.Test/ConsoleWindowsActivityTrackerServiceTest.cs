using System;
using System.Collections.Generic;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Services;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
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
            Assert.IsInstanceOf(typeof(AbstractService), _consoleWindowsActivityTrackerService);
        }

        [Test]
        public void Test_Constructor_Parameters()
        {
            CollectionAssert.AreEqual(_dependencyInjectionModules, _consoleWindowsActivityTrackerService.ServiceModules);
        }

        [Test]
        public void Test_StartService_Request_WindowsSystemEventServiceManager_And_StartProcess()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
       
            var mockWindowsSystemEventServiceManager = new Mock<IWindowsSystemEventServiceManager>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsSystemEventServiceManager)))
                .Returns(mockWindowsSystemEventServiceManager.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>
                    (_consoleWindowsActivityTrackerService, "StartService", mockServiceProvider.Object);

            mockWindowsSystemEventServiceManager.Verify(e=>e.Start(), Times.Once);
            mockWindowsSystemEventServiceManager.Verify(l => l.Stop(), Times.Never);
        }

        [Test]
        public void Test_StopService_Calls_ServiceManagerRequestOnStart_ToBe_Stop()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();

            var mockWindowsSystemEventServiceManager = new Mock<IWindowsSystemEventServiceManager>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IWindowsSystemEventServiceManager)))
                .Returns(mockWindowsSystemEventServiceManager.Object);

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>
                    (_consoleWindowsActivityTrackerService, "StartService", mockServiceProvider.Object);

            mockWindowsSystemEventServiceManager.Verify(e => e.Start(), Times.Once);
            mockWindowsSystemEventServiceManager.Verify(l => l.Stop(), Times.Never);
            mockWindowsSystemEventServiceManager.Reset();

            UnitTestUtilities
                .Helper
                .RunProtectedMethod<ConsoleWindowsActivityTrackerService>
                    (_consoleWindowsActivityTrackerService, "StopService", mockServiceProvider.Object);

            mockWindowsSystemEventServiceManager.Verify(e => e.Start(), Times.Never);
            mockWindowsSystemEventServiceManager.Verify(l => l.Stop(), Times.Once);
        }
    }
}
