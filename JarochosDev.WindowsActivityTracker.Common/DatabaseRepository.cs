using JarochosDev.Utilities.Net.NetStandard.Common.Logger;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class DatabaseRepository : IDatabaseWritableRepository<IWindowsSystemEvent>
    {
        public IMessageLogger MessageLogger { get; }

        public DatabaseRepository(IMessageLogger messageLogger)
        {
            MessageLogger = messageLogger;
        }
        public void Add(IWindowsSystemEvent value)
        {
            var text = $"\r\nLog Entry : " +
                       $"{value.Type} " +
                       $"{value.EventMessage} " +
                       $"{value.UserName} " +
                       $"{value.MachineName} " +
                       $"{value.DateTime}";

            MessageLogger.Log(text);
        }
    }

    public interface IDatabaseWritableRepository<T>
    {
        void Add(IWindowsSystemEvent value);
    }
}