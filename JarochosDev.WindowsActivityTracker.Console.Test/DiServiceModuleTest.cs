using JarochosDev.WindowsActivityTracker.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Console.Test
{
    class DiServiceModuleTest
    {
        private ServiceProvider _buildServiceProvider;

        // _windowsServiceListener = serviceProvider.GetService<IWindowsSystemEventListener>();
        //var databaseSystemEventLogger = serviceProvider.GetService<DatabaseSystemEventObserver>();
        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            var diServiceModule = new DiServiceModule();
            diServiceModule.Register(serviceCollection);
            _buildServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public void Test_Module_Register_WindowsSystemEventListener_With_Dependencies()
        {
            var windowsSystemEventListener = _buildServiceProvider.GetService<IWindowsSystemEventListener>();
            Assert.IsNotNull(windowsSystemEventListener);
            Assert.IsInstanceOf<WindowsSystemEventListener>(windowsSystemEventListener);
        }

        [Test]
        public void Test_Module_Register_DatabaseSystemEventLogger_With_Dependencies()
        {
            var windowsSystemEventListener = _buildServiceProvider.GetService<DatabaseSystemEventObserver>();
            Assert.IsNotNull(windowsSystemEventListener);
        }
    }
}
