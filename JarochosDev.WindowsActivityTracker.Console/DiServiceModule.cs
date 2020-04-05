using System;
using System.IO;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Console
{
    public class DiServiceModule : IServiceModule
    {
        public void Register(IServiceCollection serviceCollection)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var combine = Path.Combine(folderPath, @"JarochosDev\WindowsActivityTracker");
            string fileName = "log.txt";

            var fileTextLogger = new TxtFileMessageLogger(combine, fileName);

            serviceCollection
                .AddScoped<IWindowsSystemEventListener, WindowsSystemEventListener>()
                .AddScoped<IObserver<IWindowsSystemEvent>, DatabaseSystemEventObserver >()
                .AddScoped<IDatabaseWritableRepository<IWindowsSystemEvent>, DatabaseRepository>()
                .AddScoped<IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent>, PowerModeChangedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>, SessionEndedEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>, SessionSwitchEventArgsToWindowsSystemEventConverter>()
                .AddScoped<IMessageLogger,TxtFileMessageLogger>(d=> fileTextLogger)
                .AddScoped<IObserver<IWindowsSystemEvent>>(l => new WindowsServiceEventLoggerObserver(new WindowsServiceMessageLogger("WindowsActivityTracker")))
                .AddScoped(l => new WindowsServiceMessageLogger("WindowsActivityTracker"));
        }
    }
}