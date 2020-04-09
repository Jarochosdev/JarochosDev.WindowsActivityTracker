using System;
using System.Collections.Generic;
using System.ServiceProcess;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.WindowsServices;
using JarochosDev.WindowsActivityTracker.Common.Models;
using JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using JarochosDev.WindowsActivityTracker.WindowsService.Utils;


namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    partial class WindowsActivityTrackerService : DebuggableServiceBase
    {
        private IThreadApplicationRunnerProcess _process;
        public IServiceModule CoreServiceModule { get; }
        public IServiceModule WindowsServiceModule { get; }

        public IServiceProviderBuilder ServiceProviderBuilder { get; }
        public PowerBroadcastToWindowsSystemEvent PowerBroadcastToWindowsSystemEvent { get; }
        public SessionChangeToWindowsSystemEvent SessionChangeToWindowsSystemEvent { get; }
        public IEnumerable<IObserver<IWindowsSystemEvent>> WindowsSystemEventObservers { get; }

        public WindowsActivityTrackerService(IServiceModule coreServiceModule, 
            IServiceModule windowsServiceModule, 
            IServiceProviderBuilder serviceProviderBuilder, 
            PowerBroadcastToWindowsSystemEvent powerBroadcastToWindowsSystemEvent, 
            SessionChangeToWindowsSystemEvent sessionChangeToWindowsSystemEvent, 
            IEnumerable<IObserver<IWindowsSystemEvent>> windowsSystemEventObservers)
        {
            CoreServiceModule = coreServiceModule;
            WindowsServiceModule = windowsServiceModule;
            ServiceProviderBuilder = serviceProviderBuilder;
            PowerBroadcastToWindowsSystemEvent = powerBroadcastToWindowsSystemEvent;
            SessionChangeToWindowsSystemEvent = sessionChangeToWindowsSystemEvent;
            WindowsSystemEventObservers = windowsSystemEventObservers;
        }

        protected override void OnStart(string[] args)
        {
            ServiceProviderBuilder.AddServiceModule(CoreServiceModule);
            ServiceProviderBuilder.AddServiceModule(WindowsServiceModule);
            var serviceProvider = ServiceProviderBuilder.Build();

            _process = serviceProvider.GetService(typeof(IThreadApplicationRunnerProcess)) as IThreadApplicationRunnerProcess;
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
                $"SessionShutDownEvent:None",
                WindowsSystemEventType.StopWorking);

            NotifyObservers(windowsSystemEvent);
            base.OnShutdown();
        }

        private void NotifyObservers(IWindowsSystemEvent windowsSystemEvent)
        {
            //windowsSystemEvent.UserName = Machine.Instance().GetUsername();
            foreach (var windowsSystemEventObserver in WindowsSystemEventObservers)
            {
                windowsSystemEventObserver.OnNext(windowsSystemEvent);
            }
        }

        internal void OnStartDebug(string[] args)
        {
            this.OnStart(args);
        }
    }
}
