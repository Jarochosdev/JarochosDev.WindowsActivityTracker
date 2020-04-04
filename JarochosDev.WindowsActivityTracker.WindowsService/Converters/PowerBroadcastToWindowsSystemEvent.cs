﻿using System.ServiceProcess;
using JarochosDev.Utilities.NetStandard.Common.Converter;
using JarochosDev.Utilities.NetStandard.Common.Proxy;
using JarochosDev.WindowsActivityTracker.Common;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Converters
{
    public class PowerBroadcastToWindowsSystemEvent : IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent>
    {
        public IWindowsSystemEvent Convert(PowerBroadcastStatus item)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(item);
            var eventMessage = $"PowerBroadcastEvent:{item}";
            return new WindowsSystemEvent(eventMessage, windowsSystemEventType, ProxyDateTime.Instance().Now(),
                ProxyEnvironment.Instance().UserName, ProxyEnvironment.Instance().MachineName);
        }

        private WindowsSystemEventType GetWindowsSystemEventType(PowerBroadcastStatus item)
        {
            switch (item)
            {
                case PowerBroadcastStatus.BatteryLow:
                case PowerBroadcastStatus.OemEvent:
                case PowerBroadcastStatus.PowerStatusChange:
                case PowerBroadcastStatus.QuerySuspendFailed:
                    return WindowsSystemEventType.None;
                case PowerBroadcastStatus.QuerySuspend:
                case PowerBroadcastStatus.Suspend:
                    return WindowsSystemEventType.StopWorking;
                case PowerBroadcastStatus.ResumeAutomatic:
                case PowerBroadcastStatus.ResumeCritical:
                case PowerBroadcastStatus.ResumeSuspend:
                    return WindowsSystemEventType.StartWorking;
                default:
                    return WindowsSystemEventType.NotImplemented;
            }
        }
    }
}