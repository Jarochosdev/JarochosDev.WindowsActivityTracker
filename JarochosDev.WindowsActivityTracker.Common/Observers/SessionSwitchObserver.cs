using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.Observers
{
    public class SessionSwitchObserver : IObserver<SessionSwitchEventArgs>
    {
        public IWindowsSystemEventHandler WindowsSystemEventHandler { get; }
        public IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> SessionSwitchToSystemEventConverter { get; }

        public SessionSwitchObserver(IWindowsSystemEventHandler windowsSystemEventHandler, IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> sessionSwitchToSystemEventConverter)
        {
            WindowsSystemEventHandler = windowsSystemEventHandler;
            SessionSwitchToSystemEventConverter = sessionSwitchToSystemEventConverter;
        }

        public void OnCompleted()
        {
            //Not Implemented
        }

        public void OnError(Exception error)
        {
            //Not Implemented
        }

        public void OnNext(SessionSwitchEventArgs value)
        {
            WindowsSystemEventHandler.Handle(SessionSwitchToSystemEventConverter.Convert(value));
        }
    }
}