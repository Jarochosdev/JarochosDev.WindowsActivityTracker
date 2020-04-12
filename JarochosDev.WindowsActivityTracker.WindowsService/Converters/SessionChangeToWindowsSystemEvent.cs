using System.ServiceProcess;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Utilities;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Converters
{
    public class SessionChangeToWindowsSystemEvent : IObjectConverter<SessionChangeDescription, IWindowsSystemEvent>
    {
        public IWindowsSystemEventConstructor WindowsSystemEventConstructor { get; }

        public SessionChangeToWindowsSystemEvent(IWindowsSystemEventConstructor windowsSystemEventConstructor)
        {
            WindowsSystemEventConstructor = windowsSystemEventConstructor;
        }
        public IWindowsSystemEvent Convert(SessionChangeDescription item)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(item.Reason);
            var eventMessage = $"SessionChangeEvent:{item.Reason}";
            return WindowsSystemEventConstructor.Construct(eventMessage, windowsSystemEventType);
        }

        private WindowsSystemEventType GetWindowsSystemEventType(SessionChangeReason reason)
        {
            switch (reason)
            {
                case SessionChangeReason.ConsoleConnect:
                case SessionChangeReason.RemoteConnect:
                case SessionChangeReason.SessionLogon:
                case SessionChangeReason.SessionRemoteControl:
                case SessionChangeReason.SessionUnlock:
                    return WindowsSystemEventType.StartWorking;
                case SessionChangeReason.ConsoleDisconnect:
                case SessionChangeReason.RemoteDisconnect:
                case SessionChangeReason.SessionLock:
                case SessionChangeReason.SessionLogoff:
                    return WindowsSystemEventType.StopWorking;
                default:
                    return WindowsSystemEventType.NotImplemented;
            }
        }
    }
}
