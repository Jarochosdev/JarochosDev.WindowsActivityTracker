using System;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class DatabaseSystemEventObserver : IObserver<IWindowsSystemEvent>
    {
        public IDatabaseWritableRepository<IWindowsSystemEvent> DatabaseWritableRepository { get; }

        public DatabaseSystemEventObserver(IDatabaseWritableRepository<IWindowsSystemEvent> databaseWritableRepository)
        {
            DatabaseWritableRepository = databaseWritableRepository;
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
            DatabaseWritableRepository.Add(value);
        }
    }
}