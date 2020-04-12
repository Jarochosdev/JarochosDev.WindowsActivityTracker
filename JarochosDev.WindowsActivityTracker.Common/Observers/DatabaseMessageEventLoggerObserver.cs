using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Loggers;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.Observers
{
    public class DatabaseMessageEventLoggerObserver : IObserver<IWindowsSystemEvent>
    {
        private WindowsSystemEventDataSource _dataSource;
        public IDatabaseConnectionStringConfiguration DatabaseConnectionStringConfiguration { get; }
        public IMessageLogger MessageLogger { get; }
        internal IWindowsSystemEventDataSourceFactory WindowsSystemEventDataSourceFactory { get; }
        public DatabaseMessageEventLoggerObserver(IDatabaseConnectionStringConfiguration databaseConnectionStringConfiguration, IMessageLogger messageLogger) : this(databaseConnectionStringConfiguration, messageLogger, new WindowsSystemEventDataSourceFactory())
        {
           
        }

        internal DatabaseMessageEventLoggerObserver(IDatabaseConnectionStringConfiguration databaseConnectionStringConfiguration, IMessageLogger messageLogger, IWindowsSystemEventDataSourceFactory windowsSystemEventDataSourceFactory)
        {
            DatabaseConnectionStringConfiguration = databaseConnectionStringConfiguration;
            MessageLogger = messageLogger;
            WindowsSystemEventDataSourceFactory = windowsSystemEventDataSourceFactory;
        }

        public void OnCompleted()
        {
            //Not Implemented
        }

        public void OnError(Exception error)
        {
           //Not Implemented
        }

        public void OnNext(IWindowsSystemEvent value)
        {
            try
            {
                var connectionString = DatabaseConnectionStringConfiguration.GetConnectionString();

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    if (_dataSource == null)
                    {
                        _dataSource = WindowsSystemEventDataSourceFactory.GetDataSource(connectionString);
                    }

                    _dataSource.Create(value);
                }
            }
            catch (Exception e)
            {
                MessageLogger.Log(e.Message);
            }
            
        }
    }
}
