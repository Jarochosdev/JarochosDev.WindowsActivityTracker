using JarochosDev.Utilities.Net.NetStandard.Common.DependencyInjection;
using JarochosDev.Utilities.Net.NetStandard.Common.WindowsServices;
using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.WindowsService.Converters;
using JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection;
using JarochosDev.WindowsActivityTracker.WindowsService.Utilities;

namespace JarochosDev.WindowsActivityTracker.WindowsService
{
    public static class Program
    {
        public static void Main()
        {
            var windowsSystemEventConstructor = new WindowsSystemEventConstructor(new WindowsServiceUserNameExtractor());
            var windowsActivityTrackerService = new WindowsActivityTrackerService(
                new DiWindowsServiceModule(new EnvironmentDatabaseConfiguration()), 
                new ServiceProviderBuilder(),
                new PowerBroadcastToWindowsSystemEvent(windowsSystemEventConstructor), 
                new SessionChangeToWindowsSystemEvent(windowsSystemEventConstructor));
            WindowsServiceRunner.Instance().Run(new DebuggableServiceBase[] { windowsActivityTrackerService });  
        }
    }
}

    
