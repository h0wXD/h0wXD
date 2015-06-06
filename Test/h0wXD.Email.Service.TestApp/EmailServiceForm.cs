using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using h0wXD.Email.Service.Interfaces;

namespace h0wXD.Email.Service.TestApp
{
    public partial class EmailServiceForm : Form
    {
        private readonly IEmailDaemon _emailDaemon;
        private readonly Thread _thread;

        public EmailServiceForm(IEmailDaemon emailDaemon)
        {
            InitializeComponent();
            _emailDaemon = emailDaemon;

            _thread = new Thread(ThreadProc);
            _thread.Start();
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            if (ControlButton.Text == "Pause")
            {
                _emailDaemon.Pause();
                ControlButton.Text = "Continue";
            }
            else
            {
                _emailDaemon.Continue();
                ControlButton.Text = "Pause";
            }
        }
        
        private void ThreadProc()
        {
            try
            {
                _emailDaemon.Execute();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                var unknownError = ex.Message + System.Environment.NewLine + ex.StackTrace;
                Debug.WriteLine(unknownError);
                MessageBox.Show(unknownError, "Unknown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EmailServiceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _thread.Abort();
        }
    }
}
