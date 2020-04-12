using System;
using System.Collections.Generic;
using System.Linq;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner;
using JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection;
using JarochosDev.WindowsActivityTracker.WindowsService.FormObjects;
using JarochosDev.WindowsActivityTracker.WindowsService.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Test.DepedencyInjection
{
    class DiWindowsServiceModuleTest
    {
        private ServiceProvider _buildServiceProvider;
        private Mock<IDatabaseConnectionStringConfiguration> _mockDatabaseConnectionStringConfiguration;

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            _mockDatabaseConnectionStringConfiguration = new Mock<IDatabaseConnectionStringConfiguration>();
            var diServiceModule = new DiWindowsServiceModule(_mockDatabaseConnectionStringConfiguration.Object);
            diServiceModule.Register(serviceCollection);
            _buildServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public void Test_Module_Register_ThreadFormApplicationRunnerProcess_With_Dependencies()
        {
            var threadFormApplicationRunnerProcess = _buildServiceProvider.GetService<IThreadFormApplicationRunnerProcess>();
            Assert.IsNotNull(threadFormApplicationRunnerProcess);
            Assert.IsInstanceOf<ThreadFormApplicationRunnerProcess>(threadFormApplicationRunnerProcess);
            var formApplicationRunnerProcess = threadFormApplicationRunnerProcess as ThreadFormApplicationRunnerProcess;
            Assert.IsInstanceOf<WindowsSystemEventListenerForm>(formApplicationRunnerProcess.Form);
            var windowsSystemEventListenerForm = formApplicationRunnerProcess.Form as WindowsSystemEventListenerForm;
            Assert.IsInstanceOf<WindowsSystemEventServiceManager>(windowsSystemEventListenerForm.WindowsSystemEventServiceManager);
            var windowsSystemEventServiceManager = windowsSystemEventListenerForm.WindowsSystemEventServiceManager as WindowsSystemEventServiceManager;
            AssertObservers(windowsSystemEventServiceManager.Observers);
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

        [Test]
        public void Test_UserNameExtractor_Registered_Is_WindowsServiceExtractor()
        {
            var userNameExtractor = _buildServiceProvider.GetService(typeof(IUserNameExtractor));
            Assert.IsInstanceOf<WindowsServiceUserNameExtractor>(userNameExtractor);
        }
    }
}
