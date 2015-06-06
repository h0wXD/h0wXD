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
        private readonly IDirectoryWatcher _directoryWatcher;
        private readonly IEmailDao _emailDao;
        private readonly IEmailMessageParser _emailMessageParser;
        private readonly ILogger _logger;
        private readonly bool _archiveEmails;

        private readonly string _mailToAddress;
        private readonly string _mailFromAddress;
        private readonly string _mailFromDisplayName;
        private readonly Dictionary<int, bool> _processingStatus = new Dictionary<int, bool>();
        
        public EmailManager(ISettings settings, IDirectoryWatcher directoryWatcher, IEmailDao emailDao, IEmailMessageParser emailMessageParser, ILogger logger)
        {
            _emailDao = emailDao;
            _emailMessageParser = emailMessageParser;
            _logger = logger;
            _directoryWatcher = directoryWatcher;
            _archiveEmails = settings.Read(TechnicalConstants.Settings.ArchiveProcessed, false);
            _mailToAddress = settings.Read(TechnicalConstants.Settings.MailTo, String.Empty);
            _mailFromAddress = settings.Read(TechnicalConstants.Settings.MailFrom, String.Empty);
            _mailFromDisplayName = settings.Read(TechnicalConstants.Settings.MailFromDisplay, String.Empty);
        }

        public void ProcessEmail(string fileName)
        {
            var hashCode = fileName.GetHashCode();

            if (_emailDao.IsProcessed(fileName) ||
                !StartProcessing(hashCode))
            {
                return;
            }

            try
            {
                var fileContents = _emailDao.Load(fileName);
                var emailMessage = _emailMessageParser.Parse(fileContents);

                _logger.Info("Parsed file {0}", fileName);
#if DEBUG
                // Very little performance boost, as Conditional("DEBUG") does not work when using interfaces.
                _logger.Debug(emailMessage.ToString());
#endif

                if (!String.IsNullOrEmpty(_mailFromAddress))
                {
                    var displayName = String.IsNullOrEmpty(_mailFromDisplayName) ? emailMessage.From.DisplayName : _mailFromDisplayName;
                    emailMessage.From = new MailAddress(_mailFromAddress, displayName);
                }
                if (!String.IsNullOrEmpty(_mailToAddress))
                {
                    var to = emailMessage.To.Count > 0 ? emailMessage.To[0] : emailMessage.CC.Count > 0 ? emailMessage.CC[0] : emailMessage.Bcc.Count > 0 ? emailMessage.Bcc[0] : null;
                    if (to != null)
                    {
                        emailMessage.To.Clear();
                        emailMessage.CC.Clear();
                        emailMessage.Bcc.Clear();
                        emailMessage.To.Add(new MailAddress(_mailToAddress, to.DisplayName));
                    }
                }

                if (_emailDao.Send(emailMessage))
                {
                    if (_archiveEmails)
                    {
                        _emailDao.MoveToArchive(fileName);
                    }
                    else
                    {
                        _emailDao.Delete(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to parse email {0}:\n{1}\n{2}", fileName, ex.Message, ex.StackTrace);
                _emailDao.MoveToError(fileName);
            }

            SetProcessing(hashCode, false);
        }

        private bool StartProcessing(int hashCode)
        {
            lock (_processingStatus)
            {
                if (!_processingStatus.ContainsKey(hashCode) ||
                    _processingStatus[hashCode] == false)
                {
                    _processingStatus[hashCode] = true;
                    return true;
                }

                return false;
            }
        }

        private void SetProcessing(int hashCode, bool processingStatus)
        {
            lock (_processingStatus)
            {
                _processingStatus[hashCode] = processingStatus;
            }
        }

        public void ProcessExistingEmails()
        {
            foreach (var directory in _directoryWatcher.Directories)
            {
                var fileMasks = directory.FileMask.Split(';');

                foreach (var fileMask in fileMasks)
                {
                    var emailFiles = _emailDao.FindEmailsByFileMask(directory.Path, fileMask);

                    foreach (var emailFile in emailFiles)
                    {
                        ProcessEmail(emailFile);
                    }
                }
            }
        }
    }
}
