using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner;
using JarochosDev.WindowsActivityTracker.WindowsService.FormObjects;
using Microsoft.Extensions.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection
{
    public class DiWindowsServiceModule: DiCoreServiceModule
    {
        public override void Register(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IThreadFormApplicationRunnerProcess, ThreadFormApplicationRunnerProcess>()
                .AddScoped<WindowsSystemEventListenerForm>();

            base.Register(serviceCollection);
        }
    }
}