using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JarochosDev.Utilities.Net.NetStandard.Common.Converter;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.WindowsService.Utils;
using Microsoft.Win32;

namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    public partial class HiddenForm : Form
    {
        private List<IDisposable> _disposables;
        private List<IObserver<IWindowsSystemEvent>> _observers;
        internal IReadOnlyCollection<IObserver<IWindowsSystemEvent>> Observers => _observers.AsReadOnly();
        internal IReadOnlyCollection<IDisposable> Unsubscribers => _disposables.AsReadOnly();

        public IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> SessionEndedToWindowsSystemConverter { get; }
        public IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> PowerModeToWindowsSystemConverter { get; }
        public IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> SessionSwitchToWindowsSystemConverter { get; }

        public HiddenForm(IObjectConverter<SessionEndedEventArgs, IWindowsSystemEvent> sessionEndedToWindowsSystemConverter,
            IObjectConverter<PowerModeChangedEventArgs, IWindowsSystemEvent> powerModeToWindowsSystemConverter,
            IObjectConverter<SessionSwitchEventArgs, IWindowsSystemEvent> sessionSwitchToWindowsSystemConverter)
        {
            SessionEndedToWindowsSystemConverter = sessionEndedToWindowsSystemConverter;
            PowerModeToWindowsSystemConverter = powerModeToWindowsSystemConverter;
            SessionSwitchToWindowsSystemConverter = sessionSwitchToWindowsSystemConverter;
            _observers = new List<IObserver<IWindowsSystemEvent>>();
            _disposables = new List<IDisposable>();
            InitializeComponent();
        }

        private void HiddenForm_Load(object sender, EventArgs e)
        {
            
            SystemEvents.SessionEnded += SystemEventsOnSessionEnded;

            SystemEvents.SessionEnding += SystemEventsOnSessionEnding;
            SystemEvents.TimeChanged += SystemEventsOnTimeChanged;

            SystemEvents.PowerModeChanged += SystemEventsOnPowerModeChanged;
            SystemEvents.SessionSwitch += OnSessionSwitch;
        }

        private void SystemEventsOnSessionEnded(object sender, SessionEndedEventArgs e)
        {
            var windowsSystemEvent = SessionEndedToWindowsSystemConverter.Convert(e);
            NotifyObservers(windowsSystemEvent);
        }


        private void SystemEventsOnTimeChanged(object sender, EventArgs e)
        {
            var windowsSystemEvent = new WindowsSystemEvent("the time change", WindowsSystemEventType.None, DateTime.Now, "", "");
            NotifyObservers(windowsSystemEvent);
        }

        private void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            var windowsSystemEvent = new WindowsSystemEvent($"SystemEventSessionending:{e.Reason}", WindowsSystemEventType.None, DateTime.Now, "", "");
            NotifyObservers(windowsSystemEvent);
        }

        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            var windowsSystemEvent = SessionSwitchToWindowsSystemConverter.Convert(e);
            NotifyObservers(windowsSystemEvent);
        }

        private void SystemEventsOnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            var windowsSystemEvent = PowerModeToWindowsSystemConverter.Convert(e);
            NotifyObservers(windowsSystemEvent);
        }

        private void NotifyObservers(IWindowsSystemEvent windowsSystemEvent)
        {
            windowsSystemEvent.UserName = Machine.Instance().GetUsername();
            _observers.ForEach(o =>
            {
                o.OnNext(windowsSystemEvent);
            });
        }

        private void HiddenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.SessionEnded -= SystemEventsOnSessionEnded;

            SystemEvents.SessionEnding -= SystemEventsOnSessionEnding;
            SystemEvents.TimeChanged -= SystemEventsOnTimeChanged;

            SystemEvents.PowerModeChanged -= SystemEventsOnPowerModeChanged;
            SystemEvents.SessionSwitch -= OnSessionSwitch;

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

    }

    partial class HiddenForm : IObservable<IWindowsSystemEvent>
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HiddenForm";
            this.Text = "HiddenForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.HiddenForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HiddenForm_FormClosing);
            this.ResumeLayout(false);
        }

        public IDisposable Subscribe(IObserver<IWindowsSystemEvent> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            var unsubscriber = new Unsubscriber<IWindowsSystemEvent>(_observers, observer);
            _disposables.Add(unsubscriber);

            return unsubscriber;
        }
    }
}