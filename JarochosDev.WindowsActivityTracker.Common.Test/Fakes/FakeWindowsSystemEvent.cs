using System;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.Test.Fakes
{
    internal class FakeWindowsSystemEvent : IWindowsSystemEvent
    {
        public string EventMessage { get; set; }
        public WindowsSystemEventType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
    }
}