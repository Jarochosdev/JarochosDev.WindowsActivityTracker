using System;
using System.Collections.Generic;
using System.IO;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Console
{
    public class DiServiceModule : IServiceModule
    {
        public void Register(IServiceCollection serviceCollection)
        {
            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                new TextMessageEventLoggerObserver(new WindowsServiceMessageLogger(AppConstants.LOGGING_APPLICATION_NAME))
            };

            serviceCollection
                .AddScoped<IWindowsSystemEventListener, WindowsSystemEventListener>()
                .AddScoped<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>, PowerModeChangedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>, SessionEndedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>, SessionSwitchEventArgsToWindowsSystemEventConverter>()
                .AddScoped(o => observers);
        }
    }
}