using System;
using System.Collections.Generic;
using System.Linq;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test.DependencyInjection
{
    class DiServiceModuleTest
    {
        private ServiceProvider _buildServiceProvider;

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            var diServiceModule = new DiCoreServiceModule();
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
            AssertObsrvers(windowsSystemEventServiceManager.Observers);
        }

        [Test]
        public void Test_Module_Register_A_ListOfObservers_With_Dependencies()
        {
            AssertObsrvers(_buildServiceProvider.GetService<IEnumerable<IObserver<IWindowsSystemEvent>>>());
        }

        private static void AssertObsrvers(IEnumerable<IObserver<IWindowsSystemEvent>> observers)
        {
            Assert.IsNotNull(observers);
            Assert.AreEqual(1, observers.Count());
            Assert.IsInstanceOf<TextMessageEventLoggerObserver>(observers.ElementAt(0));
            var textMessageEventLoggerObserver = observers.ElementAt(0) as TextMessageEventLoggerObserver;
            Assert.IsInstanceOf<WindowsServiceMessageLogger>(textMessageEventLoggerObserver.MessageLogger);
            var windowsServiceMessageLogger = textMessageEventLoggerObserver.MessageLogger as WindowsServiceMessageLogger;
            Assert.AreEqual(AppConstants.LOGGING_APPLICATION_NAME, windowsServiceMessageLogger.SourceName);
        }
    }
}
