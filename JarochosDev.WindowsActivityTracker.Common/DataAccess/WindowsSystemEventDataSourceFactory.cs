namespace JarochosDev.WindowsActivityTracker.Common.DataAccess
{
    public class WindowsSystemEventDataSourceFactory : IWindowsSystemEventDataSourceFactory
    {
        public WindowsSystemEventDataSource GetDataSource(string connectionString)
        {
            return new WindowsSystemEventDataSource(connectionString);
        }
    }
}