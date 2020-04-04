using System;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public interface IWindowsSystemEvent
    {
        string EventMessage { get; }
        WindowsSystemEventType Type { get; }
        DateTime DateTime { get; }
        string UserName { get; set; }
        string MachineName { get; }
    }
}