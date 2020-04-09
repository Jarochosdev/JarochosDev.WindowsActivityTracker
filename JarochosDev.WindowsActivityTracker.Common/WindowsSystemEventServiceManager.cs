using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.Processes;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class WindowsSystemEventServiceManager : IWindowsSystemEventServiceManager
    {
        private readonly List<IDisposable> _subscribedItems;
        private bool _isStarted;

        public IWindowsSystemEventListener WindowsSystemEventListener { get; }
        public IEnumerable<IObserver<IWindowsSystemEvent>> Observers { get; }

        public WindowsSystemEventServiceManager(IWindowsSystemEventListener windowsSystemEventListener,
            IEnumerable<IObserver<IWindowsSystemEvent>> observers)
        {
            WindowsSystemEventListener = windowsSystemEventListener;
            Observers = observers;
            _subscribedItems = new List<IDisposable>();
        }

        public IFinalizerProcess Start()
        {
            if (!_isStarted)
            {
                foreach (var observer in Observers)
                {
                    _subscribedItems.Add(WindowsSystemEventListener.Subscribe(observer));
                }

                WindowsSystemEventListener.Start();
                _isStarted = true;
            }

            return this;
        }

        public void Stop()
        {
            if (_isStarted)
            {
                foreach (var subscribedItem in _subscribedItems)
                {
                    subscribedItem.Dispose();
                }

                WindowsSystemEventListener.Stop();
                _isStarted = false;
            }
        }
    }
}