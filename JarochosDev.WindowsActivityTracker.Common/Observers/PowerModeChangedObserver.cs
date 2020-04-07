using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.Observers
{
    public class PowerModeChangedObserver : IObserver<PowerModeChangedEventArgs>
    {
        public IWindowsSystemEventHandler WindowsSystemEventHandler { get; }
        public IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> PowerModeToSystemEventConverter { get; }

        public PowerModeChangedObserver(IWindowsSystemEventHandler windowsSystemEventHandler, IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> powerModeToSystemEventConverter)
        {
            WindowsSystemEventHandler = windowsSystemEventHandler;
            PowerModeToSystemEventConverter = powerModeToSystemEventConverter;
        }

        public void OnCompleted()
        {
            //Not Implemented
        }

        public void OnError(Exception error)
        {
            //Not Implemented
        }

        public void OnNext(PowerModeChangedEventArgs value)
        {
            WindowsSystemEventHandler.Handle(PowerModeToSystemEventConverter.Convert(value));
        }
    }
}