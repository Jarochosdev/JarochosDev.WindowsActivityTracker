using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.Services;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.Console
{
    public  class ConsoleWindowsActivityTrackerService : AbstractService 
    {
        private IWindowsSystemEventServiceManager _windowsSystemEventServiceManager;

        public ConsoleWindowsActivityTrackerService(IEnumerable<IServiceModule> dependencyInjectionModule) : 
            base(dependencyInjectionModule)
        {
        }

        protected override void StartService(IServiceProvider serviceProvider)
        {
            _windowsSystemEventServiceManager = serviceProvider.GetService<IWindowsSystemEventServiceManager>();
            _windowsSystemEventServiceManager.Start();
        }

        protected override void StopService(IServiceProvider dependencyInjectionContainer)
        {
            _windowsSystemEventServiceManager.Stop();
        }
    }
}