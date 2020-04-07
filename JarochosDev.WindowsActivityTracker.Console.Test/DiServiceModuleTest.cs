using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.WindowsActivityTracker.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Console.Test
{
    class DiServiceModuleTest
    {
        private ServiceProvider _buildServiceProvider;

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            var diServiceModule = new DiServiceModule();
            diServiceModule.Register(serviceCollection);
            _buildServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public void Test_Module_Register_WindowsSystemEventListener_With_Dependencies()
        {
            var windowsSystemEventListener = _buildServiceProvider.GetService<IWindowsSystemEventListener>();
            Assert.IsNotNull(windowsSystemEventListener);
            Assert.IsInstanceOf<WindowsSystemEventListener>(windowsSystemEventListener);
        }

        [Test]
        public void Test_Module_Register_A_ListOfObservers_With_Dependencies()
        {
            var windowsSystemEventListener = _buildServiceProvider.GetService<List<IObserver<IWindowsSystemEvent>>>();
            Assert.IsNotNull(windowsSystemEventListener);
            Assert.AreEqual(1, windowsSystemEventListener.Count);
            Assert.IsInstanceOf<TextMessageEventLoggerObserver>(windowsSystemEventListener[0]);
            var textMessageEventLoggerObserver = windowsSystemEventListener[0] as TextMessageEventLoggerObserver;
            Assert.IsInstanceOf<WindowsServiceMessageLogger>(textMessageEventLoggerObserver.MessageLogger);
            var windowsServiceMessageLogger = textMessageEventLoggerObserver.MessageLogger as WindowsServiceMessageLogger;
            Assert.AreEqual(AppConstants.LOGGING_APPLICATION_NAME, windowsServiceMessageLogger.SourceName);
        }
    }
}
