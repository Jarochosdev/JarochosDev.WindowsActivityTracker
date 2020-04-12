using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.DependencyInjection
{
    public class DiCoreServiceModule : IServiceModule
    {
        public IDatabaseConnectionStringConfiguration DatabaseConnectionStringConfiguration { get; }

        public DiCoreServiceModule(IDatabaseConnectionStringConfiguration databaseConnectionStringConfiguration)
        {
            DatabaseConnectionStringConfiguration = databaseConnectionStringConfiguration;
        }
        public virtual void Register(IServiceCollection serviceCollection)
        {
            var windowsServiceMessageLogger = new WindowsServiceMessageLogger(AppConstants.LOGGING_APPLICATION_NAME);
            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                new TextMessageEventLoggerObserver(windowsServiceMessageLogger),
                new DatabaseMessageEventLoggerObserver(DatabaseConnectionStringConfiguration,windowsServiceMessageLogger)
            };

            serviceCollection
                .AddScoped<IWindowsSystemEventListener, WindowsSystemEventListener>()
                .AddScoped<IWindowsSystemEventServiceManager, WindowsSystemEventServiceManager>()
                .AddScoped<IWindowsSystemEventConstructor, WindowsSystemEventConstructor>()
                .AddScoped<IUserNameExtractor, EnvironmentUserNameExtractor>()
                .AddScoped<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>, PowerModeChangedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>, SessionEndedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>, SessionSwitchEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IEnumerable<IObserver<IWindowsSystemEvent>>>(o => observers);
        }
    }
}