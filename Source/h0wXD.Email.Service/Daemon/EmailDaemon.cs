using System.IO;
using System.Threading;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Interfaces;
using h0wXD.IO.Interfaces;

namespace h0wXD.Email.Service.Daemon
{
    public class EmailDaemon : IEmailDaemon
    {
        private readonly IEmailManager m_emailManager;
        private readonly IDirectoryWatcher m_directoryWatcher;

        public EmailDaemon(IEncryptedConfiguration _config, IEmailManager _emailManager, IDirectoryWatcher _directoryWatcher)
        {
            m_emailManager = _emailManager;
            m_directoryWatcher = _directoryWatcher;
            
            m_directoryWatcher.AddDirectory(_config.Read<string>(TechnicalConstants.Settings.DropFolder), "*.eml");
        }

        public void Pause()
        {
            m_directoryWatcher.Stop();
        }

        public void Continue()
        {
            m_directoryWatcher.Start();
        }

        public void Execute()
        {
            m_directoryWatcher.Created += OnCreated;
            m_directoryWatcher.Start();
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
