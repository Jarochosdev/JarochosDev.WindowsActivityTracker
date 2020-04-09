using System;
using JarochosDev.WindowsActivityTracker.Common;

namespace JarochosDev.WindowsActivityTracker.WindowsService.FormObjects
{
    public partial class WindowsSystemEventListenerForm : HidedForm
    {
        public IWindowsSystemEventServiceManager WindowsSystemEventServiceManager { get; }

        public WindowsSystemEventListenerForm(IWindowsSystemEventServiceManager windowsSystemEventServiceManager)
        {
            WindowsSystemEventServiceManager = windowsSystemEventServiceManager;

            //if the logic of hiding form does not work then maybe we can implemente a class 
            //HidingFormConfiguration and call .InitializeHiding(this)
        }

        protected override void OnLoadForm(object sender, EventArgs e)
        {
            WindowsSystemEventServiceManager.Start();
        }
        
        protected override void OnCloseForm(object sender, EventArgs e)
        {
            WindowsSystemEventServiceManager.Start();
        }
    }
}