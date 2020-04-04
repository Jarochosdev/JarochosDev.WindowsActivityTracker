using System;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class WindowsSystemEvent : IWindowsSystemEvent
    {
        public string EventMessage { get; }
        public WindowsSystemEventType Type { get; }
        public DateTime DateTime { get; }
        public string UserName { get; set; }
        public string MachineName { get; }

        public WindowsSystemEvent(string eventMessage, WindowsSystemEventType type, DateTime dateTime, string userName, string machineName)
        {
            EventMessage = eventMessage;
            Type = type;
            DateTime = dateTime;
            UserName = userName;
            MachineName = machineName;
        }
    }
}