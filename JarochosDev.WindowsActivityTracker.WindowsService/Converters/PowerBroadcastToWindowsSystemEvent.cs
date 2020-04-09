using System.ServiceProcess;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Utilities;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Converters
{
    public class PowerBroadcastToWindowsSystemEvent : IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent>
    {
        public IWindowsSystemEventConstructor WindowsSystemEventConstructor { get; }

        public PowerBroadcastToWindowsSystemEvent(IWindowsSystemEventConstructor windowsSystemEventConstructor)
        {
            WindowsSystemEventConstructor = windowsSystemEventConstructor;
        }

        public IWindowsSystemEvent Convert(PowerBroadcastStatus item)
        {
            var windowsSystemEventType = GetWindowsSystemEventType(item);
            var eventMessage = $"PowerBroadcastEvent:{item}";
            return WindowsSystemEventConstructor.Construct(eventMessage, windowsSystemEventType);
        }

        private WindowsSystemEventType GetWindowsSystemEventType(PowerBroadcastStatus item)
        {
            switch (item)
            {
                case PowerBroadcastStatus.BatteryLow:
                case PowerBroadcastStatus.OemEvent:
                case PowerBroadcastStatus.PowerStatusChange:
                    return WindowsSystemEventType.None;
                case PowerBroadcastStatus.QuerySuspend:
                case PowerBroadcastStatus.Suspend:
                    return WindowsSystemEventType.StopWorking;
                case PowerBroadcastStatus.ResumeAutomatic:
                case PowerBroadcastStatus.ResumeCritical:
                case PowerBroadcastStatus.ResumeSuspend:
                case PowerBroadcastStatus.QuerySuspendFailed:
                    return WindowsSystemEventType.StartWorking;
                default:
                    return WindowsSystemEventType.NotImplemented;
            }
        }
    }
}