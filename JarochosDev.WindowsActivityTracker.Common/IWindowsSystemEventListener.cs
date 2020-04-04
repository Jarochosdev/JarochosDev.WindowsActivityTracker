using System;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public interface IWindowsSystemEventListener : IObservable<IWindowsSystemEvent>
    {
        void Start();
        void Stop();
    }
}