using System.Diagnostics;

namespace JarochosDev.WindowsActivityTracker.Common.Logger
{
    public class WindowsServiceMessageLogger : IMessageLogger
    {
        public string SourceName { get; }

        public WindowsServiceMessageLogger(string sourceName)
        {
            SourceName = sourceName;
        }
        
        public void Log(string message)
        {
            //string Event = "Application has started";

            //using (EventLog eventLog = new EventLog("Application"))
            //{
            //    eventLog.Source = "Application";
            //    eventLog.WriteEntry(Event, EventLogEntryType.Information);
            //}

            //var eventLog1 = new EventLog();

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }

            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Information);

            //eventLog1.Source = SourceName;
            //eventLog1.Log = "Application";
            
            //eventLog1.WriteEntry(message);

            //if (!EventLog.SourceExists(SourceName)) {
            //    EventLog.CreateEventSource(SourceName, message);
            //    return;
            //}

            //var eventLog = new EventLog {
            //    Source = SourceName
            //};

            //eventLog.WriteEntry(message);
        }
    }
}
