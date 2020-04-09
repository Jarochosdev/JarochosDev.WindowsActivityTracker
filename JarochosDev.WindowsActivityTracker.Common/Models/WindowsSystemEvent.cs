using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxies;

namespace JarochosDev.WindowsActivityTracker.Common.Models
{
    public class WindowsSystemEvent : IWindowsSystemEvent
    {
        public string EventMessage { get; }
        public WindowsSystemEventType Type { get; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }

        public WindowsSystemEvent(string eventMessage, WindowsSystemEventType type)
        {
            EventMessage = eventMessage;
            Type = type;
            DateTime = ProxyDateTime.Instance().Now();
            UserName = ProxyEnvironment.Instance().UserName;
            MachineName = ProxyEnvironment.Instance().MachineName;
        }
    }
}