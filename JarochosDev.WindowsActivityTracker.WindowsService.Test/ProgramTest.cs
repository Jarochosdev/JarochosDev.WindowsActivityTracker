using System;
using System.Linq;
using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.Utilities.Net.NetStandard.Common.WindowsServices;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Assert = NUnit.Framework.Assert;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Test
{
    public class ProgramTest
    {
        [Test, UseMockWindowsServiceRunner]
        public void Test_Main_CallsWindowsServiceRunner()
        {
            var mockWindowsServiceRunner = UnitTestUtilities.SingletonService.GetMockSingleton<WindowsServiceRunner>() as Mock<IWindowsServiceRunner>;
            mockWindowsServiceRunner
                .Setup(w => w.Run(It.IsAny<DebuggableServiceBase[]>()))
                .Callback<DebuggableServiceBase[]>(servicePassed =>
                {
                    Assert.AreEqual(1, servicePassed.Length);
                    Assert.IsInstanceOf<WindowsActivityTrackerService>(servicePassed.ElementAt(0));
                    var windowsActivityTrackerService = servicePassed.ElementAt(0) as WindowsActivityTrackerService;
                    Assert.IsInstanceOf<DiWindowsServiceModule>(windowsActivityTrackerService.WindowsServiceModule);
                    Assert.IsInstanceOf<ServiceProviderBuilder>(windowsActivityTrackerService.ServiceProviderBuilder);
                    Assert.IsInstanceOf<PowerBroadcastToWindowsSystemEvent>(windowsActivityTrackerService.PowerBroadcastToWindowsSystemEvent);
                    Assert.IsInstanceOf<SessionChangeToWindowsSystemEvent>(windowsActivityTrackerService.SessionChangeToWindowsSystemEvent);
                    Assert.AreEqual(1, windowsActivityTrackerService.WindowsSystemEventObservers.Count());
                    Assert.IsInstanceOf<TextMessageEventLoggerObserver>(windowsActivityTrackerService.WindowsSystemEventObservers.ElementAt(0));
                    var textMessageEventLoggerObserver = windowsActivityTrackerService.WindowsSystemEventObservers.ElementAt(0) as TextMessageEventLoggerObserver;
                    Assert.IsInstanceOf<WindowsServiceMessageLogger>(textMessageEventLoggerObserver.MessageLogger);
                    var windowsServiceMessageLogger = textMessageEventLoggerObserver.MessageLogger as WindowsServiceMessageLogger;
                    Assert.AreEqual(AppConstants.LOGGING_APPLICATION_NAME, windowsServiceMessageLogger.SourceName);
                });

            Program.Main();

            mockWindowsServiceRunner.VerifyAll();
        }
    }

    public class UseMockWindowsServiceRunnerAttribute : Attribute, ITestAction
    { 
        public void BeforeTest(ITest test)
        {
            UnitTestUtilities.SingletonService.CreateMockSingleton<WindowsServiceRunner, IWindowsServiceRunner>();
        }

        public void AfterTest(ITest test)
        {
            UnitTestUtilities.SingletonService.ClearMockSingleton<WindowsServiceRunner, IWindowsServiceRunner>();
        }

        public ActionTargets Targets { get; }
    }
}
