using System.ServiceProcess;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Converters
{
    public class PowerBroadcastToWindowsSystemEvent : IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent>
    {
        public IWindowsSystemEvent Convert(PowerBroadcastStatus item)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(item);
            var eventMessage = $"PowerBroadcastEvent:{item}";
            return new WindowsSystemEvent(eventMessage, windowsSystemEventType);
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