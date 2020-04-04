using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using JarochosDev.Utilities.NetStandard.Common.Converter;
using JarochosDev.Utilities.NetStandard.Common.Proxy;
using JarochosDev.WindowsActivityTracker.Common;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Converters
{
    public class SessionChangeToWindowsSystemEvent : IObjectConverter<SessionChangeDescription, IWindowsSystemEvent>
    {
        public IWindowsSystemEvent Convert(SessionChangeDescription item)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(item.Reason);
            var eventMessage = $"SessionChangeEvent:{item.Reason}";
            return new WindowsSystemEvent(eventMessage, windowsSystemEventType, ProxyDateTime.Instance().Now(),
                ProxyEnvironment.Instance().UserName, ProxyEnvironment.Instance().MachineName);
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
