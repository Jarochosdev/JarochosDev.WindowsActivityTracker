using System.Windows.Forms;

namespace JarochosDev.WindowsActivityTracker.WindowsService.FormObjects
{
    public partial class HidedForm : Form
    {
        public HidedForm()
        {
            this.InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HidedForm";
            this.Text = "HidedForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.WindowsSystemEventListenerForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HiddenForm_FormClosing);
            this.ResumeLayout(false);
        }

        private void WindowsSystemEventListenerForm_Load(object sender, System.EventArgs e)
        {
            this.OnLoadForm(sender, e);
        }

        protected virtual void OnLoadForm(object sender, System.EventArgs e) { }

        private void HiddenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.OnCloseForm(sender, e);
        }

        protected virtual void OnCloseForm(object sender, System.EventArgs e) { }

    }
}