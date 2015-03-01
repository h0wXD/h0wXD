using System;
using System.ServiceProcess;
using System.Threading;
using h0wXD.Email.Service.Interfaces;
using h0wXD.Logging.Interfaces;

namespace h0wXD.Email.Service
{
    public partial class EmailService : ServiceBase
    {
        private readonly Thread m_thread;
        private readonly IEmailDaemon m_emailDaemon;
        private readonly ILogger m_logger;

        public EmailService(IEmailDaemon _emailDaemon, ILogger _logger)
        {
            m_emailDaemon = _emailDaemon;
            m_logger = _logger;

            InitializeComponent();

            m_thread = new Thread(ThreadProc);
        }
        
        protected override void OnStart(string [] _args)
        {
            m_thread.Start();
        }

        protected override void OnStop()
        {
            m_thread.Abort();
        }

        protected override void OnPause()
        {
            m_emailDaemon.Pause();
        }

        protected override void OnContinue()
        {
            m_emailDaemon.Continue();
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
                m_logger.Fatal("EmailService ThreadProc Crashed! {0}\nStackTrace: {1}", _ex.Message, _ex.StackTrace);
            }
        }
    }
}
