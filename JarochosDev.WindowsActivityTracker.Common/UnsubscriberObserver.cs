using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JarochosDev.WindowsActivityTracker.Common.Test")]
namespace JarochosDev.WindowsActivityTracker.Common
{
    public class UnsubscriberObserver<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        internal IReadOnlyCollection<IObserver<T>> ReadOnlyObservers => _observers.AsReadOnly();
        internal IObserver<T> ObserverToUnsubscribe { get; }
        public UnsubscriberObserver(List<IObserver<T>> observers, IObserver<T> observerToUnsubscribe)
        {
            _observers = observers;
            ObserverToUnsubscribe = observerToUnsubscribe;
        }

        public void Dispose()
        {
            if (_observers.Contains(ObserverToUnsubscribe))
                _observers.Remove(ObserverToUnsubscribe);
        }
    }
}