using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.Utilities
{
    public class WindowsSystemEventConstructor : IWindowsSystemEventConstructor
    {
        public IUserNameExtractor UserNameExtractor { get; }

        public WindowsSystemEventConstructor(IUserNameExtractor userNameExtractor)
        {
            UserNameExtractor = userNameExtractor;
        }
        public IWindowsSystemEvent Construct(string eventMessage, WindowsSystemEventType type)
        {
            var windowsSystemEvent = new WindowsSystemEvent(eventMessage, type);
            windowsSystemEvent.UserName = UserNameExtractor.GetUserName();
            return windowsSystemEvent;
        }
    }
}
