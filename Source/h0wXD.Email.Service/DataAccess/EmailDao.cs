using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Interfaces;
using h0wXD.Logging.Interfaces;

namespace h0wXD.Email.Service.DataAccess
{
    public class EmailDao : IEmailDao
    {
        private readonly ILogger m_logger;
        private readonly string m_sPathToError;
        private readonly string m_sPathToArchive;
        private readonly SmtpClient m_smtpClient;

        public EmailDao(IConfiguration _config, ILogger _logger)
        {
            m_logger = _logger;
            var sDropFolderPath = _config.Read<string>(TechnicalConstants.Settings.DropFolder);
            
            m_sPathToError = Path.Combine(sDropFolderPath, TechnicalConstants.ErrorFolder);
            m_sPathToArchive = Path.Combine(sDropFolderPath, TechnicalConstants.ArchiveFolder);

            foreach (var sPath in new [] {m_sPathToError, m_sPathToArchive}.Where(sPath => !Directory.Exists(sPath)))
            {
                Directory.CreateDirectory(sPath);
            }

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

        public string [] FindEmailsByFileMask(string _sPath, string _sFileMask)
        {
            return Directory.GetFiles(_sPath, _sFileMask);
        }

        public bool IsProcessed(string _sEmailFile)
        {
            return !File.Exists(_sEmailFile);
        }

        public void MoveToError(string _sEmailFile)
        {
            MoveFile(_sEmailFile, m_sPathToError);
        }

        public void MoveToArchive(string _sEmailFile)
        {
            MoveFile(_sEmailFile, m_sPathToArchive);
        }

        private void MoveFile(string _sFile, string _sDestinationPath)
        {
            var iNumber = 1;
            var sNewFile = Path.Combine(_sDestinationPath, Path.GetFileName(_sFile));
            var sExtension = Path.GetExtension(_sFile);

            while (File.Exists(sNewFile))
            {
                sNewFile = Path.Combine(_sDestinationPath, Path.GetFileNameWithoutExtension(_sFile) + "_" + iNumber + sExtension);
                iNumber++;
            }

            m_logger.Info("Moved file to {0}", sNewFile);
            
            File.Move(_sFile, sNewFile);
        }

        public void Delete(string _sEmailFile)
        {
            m_logger.Info("Deleted file {0}", _sEmailFile);

            File.Delete(_sEmailFile);
        }

        public bool Send(MailMessage _mailMessage)
        {
            try
            {
                m_smtpClient.Send(_mailMessage);
                m_logger.Info("Email sent successfully!");
                return true;
            }
            catch (Exception _ex)
            {
                m_logger.Error("Unable to send email {0}:\n{1}\n{2}", _ex.Message, _ex.StackTrace);
                return false;
            }
        }

        public MailMessage Load(string _sFileName)
        {
            var mailMessage = new MailMessage();

            return mailMessage.Load(_sFileName) ? mailMessage : null;
        }
    }
}
