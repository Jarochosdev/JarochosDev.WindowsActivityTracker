using System;
using System.Collections.Generic;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        internal IReadOnlyCollection<IObserver<T>> ReadOnlyObservers => _observers.AsReadOnly();
        internal IObserver<T> ObserverToUnsubscribe { get; }
        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observerToUnsubscribe)
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