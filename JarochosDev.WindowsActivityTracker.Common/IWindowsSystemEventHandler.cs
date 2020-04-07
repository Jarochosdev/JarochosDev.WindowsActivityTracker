using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public interface IWindowsSystemEventHandler
    {
        void Handle(IWindowsSystemEvent windowsSystemEvent);
    }
}