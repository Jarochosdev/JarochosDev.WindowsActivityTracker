﻿using JarochosDev.Utilities.NetStandard.Common.Converter;
using JarochosDev.Utilities.NetStandard.Common.Proxy;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.WindowsSystemEventConverters
{
    public class SessionSwitchEventArgsToWindowsSystemEventConverter : IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent>
    {
        public IWindowsSystemEvent Convert(SessionSwitchEventArgs sessionSwitchEventArgs)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(sessionSwitchEventArgs.Reason);
            var eventMessage = $"SessionSwitchEvent:{sessionSwitchEventArgs.Reason}";
            return new WindowsSystemEvent(eventMessage, windowsSystemEventType, ProxyDateTime.Instance().Now(), ProxyEnvironment.Instance().UserName, ProxyEnvironment.Instance().MachineName);
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
                    return WindowsSystemEventType.StartWorking;
                case SessionSwitchReason.SessionRemoteControl:
                    return WindowsSystemEventType.None;
                default:
                    return WindowsSystemEventType.NotImplemented;
            }
        }
    }
}