using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.Utilities.Net.NetStandard.Common.WindowsServices;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection;
using JarochosDev.WindowsActivityTracker.WindowsService.Utilities;

namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    public static class Program
    {
        public static void Main()
        {
            var textMessageEventLoggerObserver = new TextMessageEventLoggerObserver(new WindowsServiceMessageLogger(AppConstants.LOGGING_APPLICATION_NAME));

            var windowsSystemEventConstructor = new WindowsSystemEventConstructor(new WindowsServiceUserNameExtractor());
            var windowsActivityTrackerService = new WindowsActivityTrackerService(new DiWindowsServiceModule(), 
                new ServiceProviderBuilder(),
                new PowerBroadcastToWindowsSystemEvent(windowsSystemEventConstructor), 
                new SessionChangeToWindowsSystemEvent(windowsSystemEventConstructor), 
                new List<IObserver<IWindowsSystemEvent>>(){textMessageEventLoggerObserver});
            WindowsServiceRunner.Instance().Run(new DebuggableServiceBase[] { windowsActivityTrackerService });  
        }
    }
}

    
