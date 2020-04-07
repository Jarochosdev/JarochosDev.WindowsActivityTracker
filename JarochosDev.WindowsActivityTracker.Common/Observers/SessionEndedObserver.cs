using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.Observers
{
    public class SessionEndedObserver : IObserver<SessionEndedEventArgs>
    {
        public IWindowsSystemEventHandler WindowsSystemEventHandler { get; }
        public IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> SessionEndedToSystemEventConverter { get; }

        public SessionEndedObserver(IWindowsSystemEventHandler windowsSystemEventHandler, IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> sessionEndedToSystemEventConverter)
        {
            WindowsSystemEventHandler = windowsSystemEventHandler;
            SessionEndedToSystemEventConverter = sessionEndedToSystemEventConverter;
        }

        public void OnCompleted()
        {
            //Not Implemented
        }

        public void OnError(Exception error)
        {
            //Not Implemented
        }

        public void OnNext(SessionEndedEventArgs value)
        {
            WindowsSystemEventHandler.Handle(SessionEndedToSystemEventConverter.Convert(value));
        }
    }
}