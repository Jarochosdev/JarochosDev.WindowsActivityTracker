using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.Common.Observers;
using JarochosDev.WindowsActivityTracker.Common.Proxies;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class WindowsSystemEventListener : IWindowsSystemEventListener, IWindowsSystemEventHandler
    {
        public IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> PowerModeToSystemEventConverter { get; }
        public IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> SessionEndedToSystemEventConverter { get; }
        public IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> SessionSwitchToSystemEventConverter { get; }
     
        private readonly List<IObserver<IWindowsSystemEvent>> _observers;
        private bool _isStarted;
        internal IReadOnlyCollection<IObserver<IWindowsSystemEvent>> Observers => _observers.AsReadOnly();
        private List<IDisposable> _proxySystemEventDisposable;
        public WindowsSystemEventListener(IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> powerModeToSystemEventConverter, IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> sessionEndedToSystemEventConverter, IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> sessionSwitchToSystemEventConverter)
        {
            _observers = new List<IObserver<IWindowsSystemEvent>>();
            _proxySystemEventDisposable = new List<IDisposable>();
            PowerModeToSystemEventConverter = powerModeToSystemEventConverter;
            SessionEndedToSystemEventConverter = sessionEndedToSystemEventConverter;
            SessionSwitchToSystemEventConverter = sessionSwitchToSystemEventConverter;
        }

        public IDisposable Subscribe(IObserver<IWindowsSystemEvent> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new UnsubscriberObserver<IWindowsSystemEvent>(_observers, observer);
        }

        public void Start()
        {
            if (!_isStarted)
            {
                _proxySystemEventDisposable.Add(ProxySystemEvents.Instance().Subscribe(new PowerModeChangedObserver(this, PowerModeToSystemEventConverter)));
                _proxySystemEventDisposable.Add(ProxySystemEvents.Instance().Subscribe(new SessionSwitchObserver(this, SessionSwitchToSystemEventConverter)));
                _proxySystemEventDisposable.Add(ProxySystemEvents.Instance().Subscribe(new SessionEndedObserver(this,SessionEndedToSystemEventConverter)));
                _isStarted = true;
            }
        }

        public void Stop()
        {
            if (_isStarted)
            {
                _proxySystemEventDisposable.ForEach(d=>d.Dispose());
                 _isStarted = false;
            }
        }

        public void Handle(IWindowsSystemEvent windowsSystemEvent)
        {
            _observers.ForEach(o =>
            {
                o.OnNext(windowsSystemEvent);
            });
        }
    }
}
