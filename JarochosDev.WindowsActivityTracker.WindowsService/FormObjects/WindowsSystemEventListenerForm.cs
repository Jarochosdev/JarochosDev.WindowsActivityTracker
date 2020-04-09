using System.Windows.Forms;
using JarochosDev.WindowsActivityTracker.Common;

namespace JarochosDev.WindowsActivityTracker.WindowsService.FormObjects
{
    public partial class WindowsSystemEventListenerForm : Form
    {
        public IWindowsSystemEventServiceManager WindowsSystemEventServiceManager { get; }

        public WindowsSystemEventListenerForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = nameof(WindowsSystemEventListenerForm);
            this.Text = nameof(WindowsSystemEventListenerForm);
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.WindowsSystemEventListenerForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HiddenForm_FormClosing);
            this.ResumeLayout(false);
        }

        public WindowsSystemEventListenerForm(IWindowsSystemEventServiceManager windowsSystemEventServiceManager)
        {
            WindowsSystemEventServiceManager = windowsSystemEventServiceManager;
        }

        private void WindowsSystemEventListenerForm_Load(object sender, System.EventArgs e)
        {
            WindowsSystemEventServiceManager.Start();
        }

        private void HiddenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowsSystemEventServiceManager.Stop();
        }
    }
}