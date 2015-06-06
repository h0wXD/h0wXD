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
        private readonly IEmailManager _emailManager;
        private readonly IDirectoryWatcher _directoryWatcher;
        private readonly ILogger _logger;

        public EmailDaemon(ISettings settings, IEmailManager emailManager, IDirectoryWatcher directoryWatcher, ILogger logger)
        {
            _emailManager = emailManager;
            _directoryWatcher = directoryWatcher;
            _logger = logger;

            _directoryWatcher.AddDirectory(settings.Read<string>(TechnicalConstants.Settings.DropFolder), "*.eml");
        }

        public void Pause()
        {
            _logger.Info("Paused service.");
            _directoryWatcher.Stop();
        }

        public void Continue()
        {
            _logger.Info("Continued service.");
            _directoryWatcher.Start();
        }

        public void Execute()
        {
            _logger.Info("Starting service...");
            _directoryWatcher.Created += OnCreated;
            _directoryWatcher.Start();
            _logger.Info("Started service.");
            _emailManager.ProcessExistingEmails();

            while (true)
            {
                Thread.Sleep(2500);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            _emailManager.ProcessEmail(e.FullPath);
        }
    }
}
