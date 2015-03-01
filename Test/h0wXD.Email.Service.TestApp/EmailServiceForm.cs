using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using h0wXD.Email.Service.Daemon;
using h0wXD.Email.Service.Interfaces;

namespace h0wXD.Email.Service.TestApp
{
    public partial class EmailServiceForm : Form
    {
        private readonly IEmailDaemon m_emailDaemon;
        private readonly Thread m_thread;

        public EmailServiceForm(IEmailDaemon _emailDaemon)
        {
            InitializeComponent();
            m_emailDaemon = _emailDaemon;

            m_thread = new Thread(ThreadProc);
            m_thread.Start();
        }

        private void ControlButton_Click(object _sender, EventArgs _e)
        {
            if (ControlButton.Text == "Pause")
            {
                m_emailDaemon.Pause();
                ControlButton.Text = "Continue";
            }
            else
            {
                m_emailDaemon.Continue();
                ControlButton.Text = "Pause";
            }
        }
        
        private void ThreadProc()
        {
            try
            {
                m_emailDaemon.Execute();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception _ex)
            {
                var sUnknownError = _ex.Message + Environment.NewLine + _ex.StackTrace;
                Debug.WriteLine(sUnknownError);
                MessageBox.Show(sUnknownError, "Unknown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
