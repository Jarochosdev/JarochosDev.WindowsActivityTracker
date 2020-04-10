using System;
using System.Collections.Generic;
using System.ServiceProcess;
using JarochosDev.Utilities.Net.NetStandard.Common.Converters;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.WindowsServices;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner;


namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    public partial class WindowsActivityTrackerService : DebuggableServiceBase
    {
        private IThreadFormApplicationRunnerProcess _process;
        public IServiceModule WindowsServiceModule { get; }

        public IServiceProviderBuilder ServiceProviderBuilder { get; }
        public IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent> PowerBroadcastToWindowsSystemEvent { get; }
        public IObjectConverter<SessionChangeDescription, IWindowsSystemEvent> SessionChangeToWindowsSystemEvent { get; }
        public IEnumerable<IObserver<IWindowsSystemEvent>> WindowsSystemEventObservers { get; }

        public WindowsActivityTrackerService(IServiceModule windowsServiceModule, 
            IServiceProviderBuilder serviceProviderBuilder,
            IObjectConverter<PowerBroadcastStatus, IWindowsSystemEvent> powerBroadcastToWindowsSystemEvent,
            IObjectConverter<SessionChangeDescription, IWindowsSystemEvent> sessionChangeToWindowsSystemEvent, 
            IEnumerable<IObserver<IWindowsSystemEvent>> windowsSystemEventObservers)
        {
            WindowsServiceModule = windowsServiceModule;
            ServiceProviderBuilder = serviceProviderBuilder;
            PowerBroadcastToWindowsSystemEvent = powerBroadcastToWindowsSystemEvent;
            SessionChangeToWindowsSystemEvent = sessionChangeToWindowsSystemEvent;
            WindowsSystemEventObservers = windowsSystemEventObservers;

            this.ServiceName = AppConstants.LOGGING_APPLICATION_NAME;
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanShutdown = true;
            this.AutoLog = false;
        }

        protected override void OnStart(string[] args)
        {
            var serviceProvider = ServiceProviderBuilder.AddServiceModule(WindowsServiceModule).Build();
            _process = serviceProvider.GetService(typeof(IThreadFormApplicationRunnerProcess)) as IThreadFormApplicationRunnerProcess;
            _process.Start();
        }

        protected override void OnStop()
        {
            _process.Stop();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            var windowsSystemEvent = SessionChangeToWindowsSystemEvent.Convert(changeDescription);
            NotifyObservers(windowsSystemEvent);
            base.OnSessionChange(changeDescription);
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            var windowsSystemEvent = PowerBroadcastToWindowsSystemEvent.Convert(powerStatus);
            NotifyObservers(windowsSystemEvent);
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            var windowsSystemEvent = new WindowsSystemEvent(
                $"ShutdownEvent:None",
                WindowsSystemEventType.StopWorking);

            NotifyObservers(windowsSystemEvent);
            base.OnShutdown();
        }

        private void NotifyObservers(IWindowsSystemEvent windowsSystemEvent)
        {
            foreach (var windowsSystemEventObserver in WindowsSystemEventObservers)
            {
                windowsSystemEventObserver.OnNext(windowsSystemEvent);
            }
        }
    }
}
