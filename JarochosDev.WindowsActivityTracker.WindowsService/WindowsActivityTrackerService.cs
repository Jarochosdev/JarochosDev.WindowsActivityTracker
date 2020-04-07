using System;
using System.Collections.Generic;
using System.ServiceProcess;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.WindowsService.Utils;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.Utilities.Net.NetStandard.Common.Logger;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxy;
using JarochosDev.WindowsActivityTracker.Common.Models;


namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    partial class WindowsActivityTrackerService : ServiceBase
    {
        public IObjectConverter<SessionChangeDescription, IWindowsSystemEvent> SessionChangeToWindowsSystemConverter { get; }
        public IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent> PowerBroadcastToWindowsSystemConverter { get; }
        public IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> SessionEndedToWindowsSystemConverter { get; }
        public IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> PowerModeToWindowsSystemConverter { get; }
        public IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> SessionSwitchToWindowsSystemConverter { get; }
        public IMessageLogger MessageLogger { get; }
        private List<IDisposable> _disposables;
        private List<IObserver<IWindowsSystemEvent>> _observers;
        private HiddenForm _hiddenForm;
        internal IReadOnlyCollection<IObserver<IWindowsSystemEvent>> Observers => _observers.AsReadOnly();
        internal IReadOnlyCollection<IDisposable> Unsubscribers => _disposables.AsReadOnly();
        public WindowsActivityTrackerService(
            IObjectConverter<SessionChangeDescription, IWindowsSystemEvent> sessionChangeToWindowsSystemConverter,
            IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent> powerBroadcastToWindowsSystemConverter,
            IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> sessionEndedToWindowsSystemConverter,
            IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> powerModeToWindowsSystemConverter,
            IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> sessionSwitchToWindowsSystemConverter,
                IMessageLogger messageLogger)
        {
            SessionChangeToWindowsSystemConverter = sessionChangeToWindowsSystemConverter;
            PowerBroadcastToWindowsSystemConverter = powerBroadcastToWindowsSystemConverter;
            SessionEndedToWindowsSystemConverter = sessionEndedToWindowsSystemConverter;
            PowerModeToWindowsSystemConverter = powerModeToWindowsSystemConverter;
            SessionSwitchToWindowsSystemConverter = sessionSwitchToWindowsSystemConverter;
            MessageLogger = messageLogger;
            InitializeComponent();

            _observers = new List<IObserver<IWindowsSystemEvent>>();
            _disposables = new List<IDisposable>();

            _hiddenForm = new HiddenForm(SessionEndedToWindowsSystemConverter, PowerModeToWindowsSystemConverter, SessionSwitchToWindowsSystemConverter);
        }

     
        protected override void OnStart(string[] args)
        {
            MessageLogger.Log("ServiceStarted");
            new Thread(() => Application.Run(_hiddenForm)).Start();
        }

        protected override void OnStop()
        {
            MessageLogger.Log("ServiceStop Start");
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _hiddenForm.Close();
            MessageLogger.Log("ServiceStop Done");
            Application.Exit();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            var windowsSystemEvent = SessionChangeToWindowsSystemConverter.Convert(changeDescription);
            NotifyObservers(windowsSystemEvent);
            base.OnSessionChange(changeDescription);
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            var windowsSystemEvent = PowerBroadcastToWindowsSystemConverter.Convert(powerStatus);
            NotifyObservers(windowsSystemEvent);
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            var windowsSystemEvent = new WindowsSystemEvent(
                $"SessionShutDownEvent:None",
                WindowsSystemEventType.StopWorking);

            NotifyObservers(windowsSystemEvent);
            base.OnShutdown();
        }

        private void NotifyObservers(IWindowsSystemEvent windowsSystemEvent)
        {
            windowsSystemEvent.UserName = Machine.Instance().GetUsername();
            _observers.ForEach(o =>
            {
                o.OnNext(windowsSystemEvent);
            });
        }

        public IDisposable Subscribe(IObserver<IWindowsSystemEvent> observer)
        {
            _hiddenForm.Subscribe(observer);

            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            var unsubscriber = new UnsubscriberObserver<IWindowsSystemEvent>(_observers, observer);
            _disposables.Add(unsubscriber);

            return unsubscriber;
        }

        internal void OnStartDebug(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            while (true)
            {
                
            }
            this.OnStop();
        }
    }
}
