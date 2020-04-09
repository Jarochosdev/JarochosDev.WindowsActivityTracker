using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.Common.Proxies
{
    public class ProxySystemEvents : IProxySystemEvents
    {
        private static IProxySystemEvents _instance;
        private static List<IObserver<SessionEndedEventArgs>> _sessionEndedEventObserver;
        internal static IReadOnlyCollection<IObserver<SessionEndedEventArgs>> SessionEndedEventObserver => _sessionEndedEventObserver?.AsReadOnly() ?? null;

        private static List<IObserver<SessionSwitchEventArgs>> _sessionSwitchEventObserver;
        internal static IReadOnlyCollection<IObserver<SessionSwitchEventArgs>> SessionSwitchEventObserver => _sessionSwitchEventObserver?.AsReadOnly() ?? null;

        private static List<IObserver<PowerModeChangedEventArgs>> _powerModeChangedEventObserver;
        internal static IReadOnlyCollection<IObserver<PowerModeChangedEventArgs>> PowerModeChangedEventObserver => _powerModeChangedEventObserver?.AsReadOnly() ?? null;

        public static IProxySystemEvents Instance()
        {
            if (_instance == null)
            {
                _instance = new ProxySystemEvents();

                _sessionEndedEventObserver = new List<IObserver<SessionEndedEventArgs>>();
                _sessionSwitchEventObserver = new List<IObserver<SessionSwitchEventArgs>>();
                _powerModeChangedEventObserver = new List<IObserver<PowerModeChangedEventArgs>>();

                SystemEvents.PowerModeChanged += SystemEventsOnPowerModeChanged;
                SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
                SystemEvents.SessionEnded += SystemEventsOnSessionEnded;
            }

            return _instance;
        }

        private static void SystemEventsOnSessionEnded(object sender, SessionEndedEventArgs e)
        {
            NotifyObservers(e);
        }

        private static void NotifyObservers(SessionEndedEventArgs sessionEndedEventArgs)
        {
            _sessionEndedEventObserver.ForEach(o=> o.OnNext(sessionEndedEventArgs));
        }

        private static void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            NotifyObservers(e);
        }

        private static void NotifyObservers(SessionSwitchEventArgs sessionSwitchEventArgs)
        {
            _sessionSwitchEventObserver.ForEach(o => o.OnNext(sessionSwitchEventArgs));
        }

        private static void SystemEventsOnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            NotifyObservers(e);
        }

        private static void NotifyObservers(PowerModeChangedEventArgs powerModeChangedEventArgs)
        {
            _powerModeChangedEventObserver.ForEach(o => o.OnNext(powerModeChangedEventArgs));
        }

        public IDisposable Subscribe(IObserver<SessionEndedEventArgs> observer)
        {
            if (!_sessionEndedEventObserver.Contains(observer))
            {
                _sessionEndedEventObserver.Add(observer);
            }

            return new UnsubscriberObserver<SessionEndedEventArgs>(_sessionEndedEventObserver, observer);
        }

        public IDisposable Subscribe(IObserver<SessionSwitchEventArgs> observer)
        {
            if (!_sessionSwitchEventObserver.Contains(observer))
            {
                _sessionSwitchEventObserver.Add(observer);
            }

            return new UnsubscriberObserver<SessionSwitchEventArgs>(_sessionSwitchEventObserver, observer);
        }

        public IDisposable Subscribe(IObserver<PowerModeChangedEventArgs> observer)
        {
            if (!_powerModeChangedEventObserver.Contains(observer))
            {
                _powerModeChangedEventObserver.Add(observer);
            }

            return new UnsubscriberObserver<PowerModeChangedEventArgs>(_powerModeChangedEventObserver,observer);
        }
    }

    public interface IProxySystemEvents : IObservable<SessionEndedEventArgs>, IObservable<SessionSwitchEventArgs>, IObservable<PowerModeChangedEventArgs>
    {
    }
}
