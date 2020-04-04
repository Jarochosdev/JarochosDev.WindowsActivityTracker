using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxy;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters
{
    public class SessionEndedEventArgsToWindowsSystemEventConverter : IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent>
    {
        public IWindowsSystemEvent Convert(SessionEndedEventArgs sessionSwitchEventArgs)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(sessionSwitchEventArgs.Reason);
            var eventMessage = $"SessionEndedEvent:{sessionSwitchEventArgs.Reason}";
            return new WindowsSystemEvent(eventMessage, windowsSystemEventType, ProxyDateTime.Instance().Now(), ProxyEnvironment.Instance().UserName, ProxyEnvironment.Instance().MachineName);
        }

        private WindowsSystemEventType GetWindowsSystemEventType(SessionEndReasons reason)
        {
            switch (reason)
            {
                case SessionEndReasons.Logoff:
                case SessionEndReasons.SystemShutdown:
                    return WindowsSystemEventType.StopWorking;
                default:
                    return WindowsSystemEventType.NotImplemented;
            }
        }
    }
}