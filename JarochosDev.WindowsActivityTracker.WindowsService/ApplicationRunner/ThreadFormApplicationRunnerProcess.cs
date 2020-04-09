using System.Threading;
using System.Windows.Forms;
using JarochosDev.Utilities.Net.NetStandard.Common.Processes;
using JarochosDev.WindowsActivityTracker.WindowsService.FormObjects;

namespace JarochosDev.WindowsActivityTracker.WindowsService.ApplicationRunner
{
    public class ThreadFormApplicationRunnerProcess : IThreadFormApplicationRunnerProcess
    {
        public Form Form { get; }

        public ThreadFormApplicationRunnerProcess(WindowsSystemEventListenerForm form)
        {
            Form = form;
        }
        public IFinalizerProcess Start()
        {
            new Thread(() => Application.Run(Form)).Start();
            return this;
        }

        public void Stop()
        {
            Form.Close();
            Application.Exit();
        }
    }
}