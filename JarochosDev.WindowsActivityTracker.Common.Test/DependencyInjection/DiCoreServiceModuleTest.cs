using System;
using System.Collections.Generic;
using System.Linq;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.DependencyInjection
{
    class DiCoreServiceModuleTest
    {
        private ServiceProvider _buildServiceProvider;
        private Mock<IDatabaseConnectionStringConfiguration> _mockDatabaseConnectionStringConfiguration;

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            _mockDatabaseConnectionStringConfiguration = new Mock<IDatabaseConnectionStringConfiguration>();
            var diServiceModule = new DiCoreServiceModule(_mockDatabaseConnectionStringConfiguration.Object);
            diServiceModule.Register(serviceCollection);
            _buildServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public void Test_Module_Register_WindowsSystemEventServiceManager_With_Dependencies()
        {
            var windowsSystemEventListener = _buildServiceProvider.GetService<IWindowsSystemEventServiceManager>();
            Assert.IsNotNull(windowsSystemEventListener);
            Assert.IsInstanceOf<WindowsSystemEventServiceManager>(windowsSystemEventListener);
            var windowsSystemEventServiceManager = windowsSystemEventListener as WindowsSystemEventServiceManager;
            AssertObservers(windowsSystemEventServiceManager.Observers);
        }

        [Test]
        public void Test_Module_Register_A_ListOfObservers_With_Dependencies()
        {
            AssertObservers(_buildServiceProvider.GetService<IEnumerable<IObserver<IWindowsSystemEvent>>>());
        }

        private void AssertObservers(IEnumerable<IObserver<IWindowsSystemEvent>> observers)
        {
            Assert.IsNotNull(observers);
            Assert.AreEqual(2, observers.Count());
            
            Assert.IsInstanceOf<TextMessageEventLoggerObserver>(observers.ElementAt(0));
            var textMessageEventLoggerObserver = observers.ElementAt(0) as TextMessageEventLoggerObserver;
            Assert.IsInstanceOf<WindowsServiceMessageLogger>(textMessageEventLoggerObserver.MessageLogger);
            var windowsServiceMessageLogger = textMessageEventLoggerObserver.MessageLogger as WindowsServiceMessageLogger;
            Assert.AreEqual(AppConstants.LOGGING_APPLICATION_NAME, windowsServiceMessageLogger.SourceName);

            Assert.IsInstanceOf<DatabaseMessageEventLoggerObserver>(observers.ElementAt(1));
            var databaseMessageEventLoggerObserver = observers.ElementAt(1) as DatabaseMessageEventLoggerObserver;
            Assert.AreSame(textMessageEventLoggerObserver.MessageLogger, databaseMessageEventLoggerObserver.MessageLogger);
            Assert.AreSame(_mockDatabaseConnectionStringConfiguration.Object, databaseMessageEventLoggerObserver.DatabaseConnectionStringConfiguration);
        }
    }
}
