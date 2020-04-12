namespace JarochosDev.WindowsActivityTracker.Common.DataAccess
{
    public interface IWindowsSystemEventDataSourceFactory
    {
        WindowsSystemEventDataSource GetDataSource(string connectionString);
    }
}