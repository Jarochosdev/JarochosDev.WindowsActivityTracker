using System;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public interface IWindowsSystemEventListener : IObservable<IWindowsSystemEvent>
    {
        void Start();
        void Stop();
    }
}