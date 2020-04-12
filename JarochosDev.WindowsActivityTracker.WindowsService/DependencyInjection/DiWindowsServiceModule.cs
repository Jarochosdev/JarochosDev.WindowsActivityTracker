using JarochosDev.WindowsActivityTracker.Common.DataAccess;
using JarochosDev.WindowsActivityTracker.Common.DependencyInjection;
using JarochosDev.WindowsActivityTracker.Common.Utilities;
using JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner;
using JarochosDev.WindowsActivityTracker.WindowsService.FormObjects;
using JarochosDev.WindowsActivityTracker.WindowsService.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace JarochosDev.WindowsActivityTracker.WindowsService.DependencyInjection
{
    public class DiWindowsServiceModule: DiCoreServiceModule
    {
        public DiWindowsServiceModule(IDatabaseConnectionStringConfiguration databaseConnectionStringConfiguration) : base(databaseConnectionStringConfiguration)
        {
        }
        public override void Register(IServiceCollection serviceCollection)
        {
            base.Register(serviceCollection);

            serviceCollection
                .AddScoped<IThreadFormApplicationRunnerProcess, ThreadFormApplicationRunnerProcess>()
                .AddScoped<IUserNameExtractor, WindowsServiceUserNameExtractor>()
                .AddScoped<WindowsSystemEventListenerForm>();
        }
    }
}