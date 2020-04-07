using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxy;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters
{
    public class SessionSwitchEventArgsToWindowsSystemEventConverter : IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>
    {
        public IWindowsSystemEvent Convert(SessionSwitchEventArgs sessionSwitchEventArgs)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(sessionSwitchEventArgs.Reason);
            var eventMessage = $"SessionSwitchEvent:{sessionSwitchEventArgs.Reason}";
            return new WindowsSystemEvent(eventMessage, windowsSystemEventType);
        }

        private WindowsSystemEventType GetWindowsSystemEventType(SessionSwitchReason reason)
        {
            switch (reason)
            {
                case SessionSwitchReason.ConsoleDisconnect:
                case SessionSwitchReason.RemoteDisconnect:
                case SessionSwitchReason.SessionLock:
                case SessionSwitchReason.SessionLogoff:
                    return WindowsSystemEventType.StopWorking;
                case SessionSwitchReason.ConsoleConnect:
                case SessionSwitchReason.RemoteConnect:
                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                case SessionSwitchReason.SessionRemoteControl:
                    return WindowsSystemEventType.StartWorking;
                default:
                    return WindowsSystemEventType.NotImplemented;
            }
        }
    }
}