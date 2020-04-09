using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.Utilities.Net.NetStandard.Common.WindowsServices;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    static class Program
    {
        static void Main()
        {
            var textMessageEventLoggerObserver = new TextMessageEventLoggerObserver(new WindowsServiceMessageLogger(AppConstants.LOGGING_APPLICATION_NAME));

            var windowsActivityTrackerService = new WindowsActivityTrackerService(new DiCoreServiceModule(), 
                new DiWindowsServiceModule(), 
                new ServiceProviderBuilder(),
                new PowerBroadcastToWindowsSystemEvent(), 
                new SessionChangeToWindowsSystemEvent(), 
                new List<IObserver<IWindowsSystemEvent>>(){textMessageEventLoggerObserver});

            WindowsServiceRunner.Instance().Run(new DebuggableServiceBase[] { windowsActivityTrackerService });   
        }
    }
}

    
