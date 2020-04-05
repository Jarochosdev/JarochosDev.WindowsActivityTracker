using System;
using System.ServiceProcess;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;

namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var windowsServiceTextLogger = new WindowsServiceMessageLogger("WindowsActivityTrackerService");
            var windowsServiceEventLoggerObserver = new WindowsServiceEventLoggerObserver(windowsServiceTextLogger);
            var windowsActivityTrackerService = new WindowsActivityTrackerService(
                new SessionChangeToWindowsSystemEvent(), 
                new PowerBroadcastToWindowsSystemEvent(), 
                new SessionEndedEventArgsToWindowsSystemEventConverter(),
                new PowerModeChangedEventArgsToWindowsSystemEventConverter(), 
                new SessionSwitchEventArgsToWindowsSystemEventConverter(), 
                windowsServiceTextLogger);
            windowsActivityTrackerService.Subscribe(windowsServiceEventLoggerObserver);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                windowsActivityTrackerService
            };
            
            if (Environment.UserInteractive)
            {
                windowsActivityTrackerService.OnStartDebug(null);
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

    
