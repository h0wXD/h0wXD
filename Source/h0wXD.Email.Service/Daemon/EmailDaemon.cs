using System.IO;
using System.Threading;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Interfaces;
using h0wXD.IO.Interfaces;
using h0wXD.Logging.Interfaces;

namespace h0wXD.Email.Service.Daemon
{
    public class EmailDaemon : IEmailDaemon
    {
        private readonly IEmailManager m_emailManager;
        private readonly IDirectoryWatcher m_directoryWatcher;
        private readonly ILogger m_logger;

        public EmailDaemon(IConfiguration _config, IEmailManager _emailManager, IDirectoryWatcher _directoryWatcher, ILogger _logger)
        {
            m_emailManager = _emailManager;
            m_directoryWatcher = _directoryWatcher;
            m_logger = _logger;

            m_directoryWatcher.AddDirectory(_config.Read<string>(TechnicalConstants.Settings.DropFolder), "*.eml");
        }

        public void Pause()
        {
            m_logger.Info("Paused service.");
            m_directoryWatcher.Stop();
        }

        public void Continue()
        {
            m_logger.Info("Continued service.");
            m_directoryWatcher.Start();
        }

        public void Execute()
        {
            m_logger.Info("Starting service...");
            m_directoryWatcher.Created += OnCreated;
            m_directoryWatcher.Start();
            m_logger.Info("Started service.");
            m_emailManager.ProcessExistingEmails();

            while (true)
            {
                Thread.Sleep(2500);
            }
        }

        private void OnCreated(object _sender, FileSystemEventArgs _e)
        {
            m_emailManager.ProcessEmail(_e.FullPath);
        }
    }
}
