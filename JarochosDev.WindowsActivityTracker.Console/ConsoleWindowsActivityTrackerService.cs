using System;
using System.Collections.Generic;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp;
using JarochosDev.Utilities.Net.NetStandard.ConsoleApp.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.Console
{
    public  class ConsoleWindowsActivityTrackerService : AbstractConsoleService 
    {
        private List<IDisposable> _subscribedItems;
        private IWindowsSystemEventListener _windowsServiceListener;

        public ConsoleWindowsActivityTrackerService(IEnumerable<IServiceModule> dependencyInjectionModule) : base(dependencyInjectionModule)
        {
            _subscribedItems = new List<IDisposable>();
        }

        protected override void StartService(IServiceProvider serviceProvider)
        {
            _windowsServiceListener = serviceProvider.GetService<IWindowsSystemEventListener>();
            var observers = serviceProvider.GetService<List<IObserver<IWindowsSystemEvent>>>();
            foreach (var observer in observers)
            {
                _subscribedItems.Add(_windowsServiceListener.Subscribe(observer));
            }
            
            _windowsServiceListener.Start();
        }

        protected override void StopService(IServiceProvider dependencyInjectionContainer)
        {
            
            foreach (var subscribedItem in _subscribedItems)
            {
                subscribedItem.Dispose();
            }

            _windowsServiceListener.Stop();
        }
    }
}