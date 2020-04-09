using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.Utilities
{
    public interface IWindowsSystemEventConstructor
    {
        IWindowsSystemEvent Construct(string eventMessage, WindowsSystemEventType type);
    }
}