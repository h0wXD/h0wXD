using System;
using System.Collections.Generic;
using System.Net.Mail;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Interfaces;
using h0wXD.Email.Service.Interfaces;
using h0wXD.IO.Interfaces;
using h0wXD.Logging.Interfaces;
using IEmailDao = h0wXD.Email.Service.Interfaces.IEmailDao;

namespace h0wXD.Email.Service.Managers
{
    public class EmailManager : IEmailManager
    {
        private readonly IDirectoryWatcher m_directoryWatcher;
        private readonly IEmailDao m_emailDao;
        private readonly IEmailMessageParser m_emailMessageParser;
        private readonly ILogger m_logger;
        private readonly bool m_bArchiveEmails;

        private readonly string m_sMailToAddress;
        private readonly string m_sMailFromAddress;
        private readonly string m_sMailFromDisplayName;
        private readonly Dictionary<int, bool> m_processingStatus = new Dictionary<int, bool>();
        
        public EmailManager(IConfiguration _config, IDirectoryWatcher _directoryWatcher, IEmailDao _emailDao, IEmailMessageParser _emailMessageParser, ILogger _logger)
        {
            m_emailDao = _emailDao;
            m_emailMessageParser = _emailMessageParser;
            m_logger = _logger;
            m_directoryWatcher = _directoryWatcher;
            m_bArchiveEmails = _config.Read(TechnicalConstants.Settings.ArchiveProcessed, false);
            m_sMailToAddress = _config.Read(TechnicalConstants.Settings.MailTo, String.Empty);
            m_sMailFromAddress = _config.Read(TechnicalConstants.Settings.MailFrom, String.Empty);
            m_sMailFromDisplayName = _config.Read(TechnicalConstants.Settings.MailFromDisplay, String.Empty);
        }

        public void ProcessEmail(string _sFileName)
        {
            var iHashCode = _sFileName.GetHashCode();

            if (m_emailDao.IsProcessed(_sFileName) ||
                !StartProcessing(iHashCode))
            {
                return;
            }

            try
            {
                var sFileContents = m_emailDao.Load(_sFileName);
                var emailMessage = m_emailMessageParser.Parse(sFileContents);

                m_logger.Info("Parsed file {0}", _sFileName);
#if DEBUG
                // Very little performance boost, as Conditional("DEBUG") does not work when using interfaces.
                m_logger.Debug(mailMessage.ToString());
#endif

                if (!String.IsNullOrEmpty(m_sMailFromAddress))
                {
                    var sDisplayName = String.IsNullOrEmpty(m_sMailFromDisplayName) ? emailMessage.From.DisplayName : m_sMailFromDisplayName;
                    emailMessage.From = new MailAddress(m_sMailFromAddress, sDisplayName);
                }
                if (!String.IsNullOrEmpty(m_sMailToAddress))
                {
                    var to = emailMessage.To.Count > 0 ? emailMessage.To[0] : emailMessage.CC.Count > 0 ? emailMessage.CC[0] : emailMessage.Bcc.Count > 0 ? emailMessage.Bcc[0] : null;
                    if (to != null)
                    {
                        emailMessage.To.Clear();
                        emailMessage.CC.Clear();
                        emailMessage.Bcc.Clear();
                        emailMessage.To.Add(new MailAddress(m_sMailToAddress, to.DisplayName));
                    }
                }

                if (m_emailDao.Send(emailMessage))
                {
                    if (m_bArchiveEmails)
                    {
                        m_emailDao.MoveToArchive(_sFileName);
                    }
                    else
                    {
                        m_emailDao.Delete(_sFileName);
                    }
                }
            }
            catch (Exception _ex)
            {
                m_logger.Error("Unable to parse email {0}:\n{1}\n{2}", _sFileName, _ex.Message, _ex.StackTrace);
                m_emailDao.MoveToError(_sFileName);
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
                    return true;
                }

                return false;
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
