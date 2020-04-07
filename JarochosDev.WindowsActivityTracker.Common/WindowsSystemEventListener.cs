using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common
{
    public class WindowsSystemEventListener : IWindowsSystemEventListener
    {
        public IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> PowerModeToSystemEventConverter { get; }
        public IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> SessionEndedToSystemEventConverter { get; }
        public IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> SessionSwitchToSystemEventConverter { get; }
     
        private readonly List<IObserver<IWindowsSystemEvent>> _observers;
        private bool _isStarted;
        internal IReadOnlyCollection<IObserver<IWindowsSystemEvent>> Observers => _observers.AsReadOnly();
        public WindowsSystemEventListener(IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> powerModeToSystemEventConverter, IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> sessionEndedToSystemEventConverter, IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> sessionSwitchToSystemEventConverter)
        {
            _observers = new List<IObserver<IWindowsSystemEvent>>();
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

            return new Unsubscriber<IWindowsSystemEvent>(_observers, observer);
        }

        public void Start()
        {
            if (!_isStarted)
            {
                SystemEvents.PowerModeChanged += OnPowerModeChange;
                SystemEvents.SessionSwitch += OnSessionSwitch;
                SystemEvents.SessionEnded += OnSessionEnded;
            }
        }

        private void NotifyObservers(IWindowsSystemEvent windowsSystemEvent)
        {
            _observers.ForEach(o =>
            {
                o.OnNext(windowsSystemEvent);
            });
        }

        private void OnSessionEnded(object sender, SessionEndedEventArgs e)
        {
            var windowsSystemEvent = SessionEndedToSystemEventConverter.Convert(e);
            NotifyObservers(windowsSystemEvent);
        }

        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            var windowsSystemEvent = SessionSwitchToSystemEventConverter.Convert(e);
            NotifyObservers(windowsSystemEvent);
        }

        private void OnPowerModeChange(object sender, PowerModeChangedEventArgs e)
        {
            var windowsSystemEvent = PowerModeToSystemEventConverter.Convert(e);
            NotifyObservers(windowsSystemEvent);
        }

        public void Stop()
        {
            if (_isStarted)
            {
                SystemEvents.PowerModeChanged -= OnPowerModeChange;
                SystemEvents.SessionSwitch -= OnSessionSwitch;
                SystemEvents.SessionEnded -= OnSessionEnded;
                _isStarted = false;
            }
        }
    }
}
