using System;
using System.Collections.Generic;
using System.Linq;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.DependencyInjection
{
    public class DiCoreServiceModule : IServiceModule
    {
        public void Register(IServiceCollection serviceCollection)
        {
            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                new TextMessageEventLoggerObserver(new WindowsServiceMessageLogger(AppConstants.LOGGING_APPLICATION_NAME))
            };

            serviceCollection
                .AddScoped<IWindowsSystemEventListener, WindowsSystemEventListener>()
                .AddScoped<IWindowsSystemEventServiceManager, WindowsSystemEventServiceManager>()
                .AddScoped<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>, PowerModeChangedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>, SessionEndedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>, SessionSwitchEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IEnumerable<IObserver<IWindowsSystemEvent>>>(o => observers);
        }
    }
}