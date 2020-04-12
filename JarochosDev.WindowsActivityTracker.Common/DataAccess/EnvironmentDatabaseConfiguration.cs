using System;

namespace JarochosDev.WindowsActivityTracker.Common.DataAccess
{
    public class EnvironmentDatabaseConfiguration : IDatabaseConnectionStringConfiguration
    {
        public string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable(AppConstants.WINDOWS_ACTIVITY_TRACKER_CONNECTION_STRING);
        }
    }
}
