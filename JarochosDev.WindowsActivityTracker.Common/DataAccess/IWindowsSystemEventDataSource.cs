using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.DataAccess
{
    internal interface IWindowsSystemEventDataSource
    {
        void Create(IWindowsSystemEvent value);
    }
}