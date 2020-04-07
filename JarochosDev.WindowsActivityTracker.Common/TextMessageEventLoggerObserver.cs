using System;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class TextMessageEventLoggerObserver : IWindowsServiceEventLogger
    {
        public IMessageLogger MessageLogger { get; }

        public TextMessageEventLoggerObserver(IMessageLogger messageLogger)
        {
            MessageLogger = messageLogger;
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
            var text = $"Log Entry : " +
                       $"{value.Type} " +
                       $"{value.EventMessage} " +
                       $"{value.UserName} " +
                       $"{value.MachineName} " +
                       $"{value.DateTime}";

            MessageLogger.Log(text);
        }
    }

    public interface IWindowsServiceEventLogger : IObserver<IWindowsSystemEvent>
    {
    }
}