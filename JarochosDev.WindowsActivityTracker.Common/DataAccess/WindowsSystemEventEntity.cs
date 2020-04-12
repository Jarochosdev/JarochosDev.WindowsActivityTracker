using System;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.DataAccess
{
    public class WindowsSystemEventEntity : IWindowsSystemEvent
    {
        public int Id { get; set; }
        public string EventMessage { get; set; }
        public WindowsSystemEventType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
    }
}
