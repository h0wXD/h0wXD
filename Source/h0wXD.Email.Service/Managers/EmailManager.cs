using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Interfaces;
using h0wXD.IO.Interfaces;
using h0wXD.Logging;
using h0wXD.Logging.Interfaces;

namespace h0wXD.Email.Service.Managers
{
    public class EmailManager : IEmailManager
    {
        private readonly IDirectoryWatcher m_directoryWatcher;
        private readonly IEmailDao m_emailDao;
        private readonly ILogger m_logger;
        private readonly SmtpClient m_smtpClient;
        private readonly bool m_bArchiveEmails;

        private readonly string m_sFromAddress;
        private readonly Dictionary<int, bool> m_processingStatus = new Dictionary<int, bool>();
        
        public EmailManager(IEncryptedConfiguration _config, IDirectoryWatcher _directoryWatcher, IEmailDao _emailDao, ILogger _logger)
        {
            m_emailDao = _emailDao;
            m_logger = _logger;
            m_directoryWatcher = _directoryWatcher;
            m_bArchiveEmails = _config.Read(TechnicalConstants.Settings.ArchiveProcessed, false);
            m_sFromAddress = _config.Read<string>(TechnicalConstants.Settings.SmtpLogin);

            var sSmtpServer = _config.Read<string>(TechnicalConstants.Settings.SmtpServer);
            var usSmtpPort = _config.Read<ushort>(TechnicalConstants.Settings.SmtpPort);
            var sSmtpLogin = _config.Read<string>(TechnicalConstants.Settings.SmtpLogin);
            var sSmtpPassword = _config.Read<string>(TechnicalConstants.Settings.SmtpPassword);

            m_smtpClient = new SmtpClient(sSmtpServer, usSmtpPort);
            m_smtpClient.EnableSsl = true;
            m_smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            m_smtpClient.UseDefaultCredentials = false;
            m_smtpClient.Credentials = new NetworkCredential(sSmtpLogin, sSmtpPassword);
        }

        public void ProcessEmail(string _sFileName)
        {
            var iHashCode = _sFileName.GetHashCode();

            if (m_emailDao.IsProcessed(_sFileName) ||
                !StartProcessing(iHashCode))
            {
                return;
            }

            var mailMessage = new MailMessage();

            try
            {
                mailMessage.Load(_sFileName);

                if (!mailMessage.From.Address.Equals(m_sFromAddress, StringComparison.InvariantCultureIgnoreCase))
                {
                    mailMessage.From = new MailAddress(m_sFromAddress, mailMessage.From.DisplayName);
                }

                m_smtpClient.Send(mailMessage);

                if (m_bArchiveEmails)
                {
                    m_emailDao.MoveToArchive(_sFileName);
                }
                else
                {
                    m_emailDao.Delete(_sFileName);
                }
            }
            catch (Exception _ex)
            {
                m_logger.Error("Unable to parse email {0}:\r\n{1}\r\n{2}", _sFileName, _ex.Message, _ex.StackTrace);
                m_emailDao.MoveToUnprocessed(_sFileName);
            }

            SetProcessing(iHashCode, false);
        }

        private bool StartProcessing(int _iHashCode)
        {
            lock (m_processingStatus)
            {
                if (!m_processingStatus.ContainsKey(_iHashCode) ||
                    m_processingStatus[_iHashCode] == false)
                {
                    m_processingStatus[_iHashCode] = true;
                    return false;
                }

                return true;
            }
        }

        private void SetProcessing(int _iHashCode, bool _bStatus)
        {
            lock (m_processingStatus)
            {
                m_processingStatus[_iHashCode] = _bStatus;
            }
        }

        public void ProcessExistingEmails()
        {
            foreach (var directory in m_directoryWatcher.Directories)
            {
                var sFileMasks = directory.FileMask.Split(';');

                foreach (var sFileMask in sFileMasks)
                {
                    var sEmailFileArray = m_emailDao.FindEmailsByFileMask(directory.Path, sFileMask);

                    foreach (var sEmailFile in sEmailFileArray)
                    {
                        ProcessEmail(sEmailFile);
                    }
                }
            }
        }
    }
}
