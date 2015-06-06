using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using h0wXD.Configuration.Interfaces;
using h0wXD.Logging.Interfaces;
using IEmailDao = h0wXD.Email.Service.Interfaces.IEmailDao;

namespace h0wXD.Email.Service.DataAccess
{
    public class EmailDao : IEmailDao
    {
        private readonly ILogger _logger;
        private readonly string _errorPath;
        private readonly string _archivePath;
        private readonly SmtpClient _smtpClient;

        public EmailDao(ISettings settings, ILogger logger)
        {
            _logger = logger;

            var dropFolderPath = settings.Read<string>(TechnicalConstants.Settings.DropFolder);
            
            _errorPath = Path.Combine(dropFolderPath, TechnicalConstants.ErrorFolder);
            _archivePath = Path.Combine(dropFolderPath, TechnicalConstants.ArchiveFolder);

            foreach (var path in new [] {_errorPath, _archivePath}.Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }

            var smtpServer = settings.Read<string>(TechnicalConstants.Settings.SmtpServer);
            var smtpPort = settings.Read<ushort>(TechnicalConstants.Settings.SmtpPort);
            var smtpLogin = settings.Read<string>(TechnicalConstants.Settings.SmtpLogin);
            var smtpPassword = settings.Read<string>(TechnicalConstants.Settings.SmtpPassword);

            _smtpClient = new SmtpClient(smtpServer, smtpPort);
            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
        }

        public string [] FindEmailsByFileMask(string directory, string fileMask)
        {
            return Directory.GetFiles(directory, fileMask);
        }

        public bool IsProcessed(string emailFilePath)
        {
            return !File.Exists(emailFilePath);
        }

        public void MoveToError(string emailFilePath)
        {
            MoveFile(emailFilePath, _errorPath);
        }

        public void MoveToArchive(string emailFilePath)
        {
            MoveFile(emailFilePath, _archivePath);
        }

        private void MoveFile(string emailFile, string destinationPath)
        {
            var counter = 1;
            var newFileName = Path.Combine(destinationPath, Path.GetFileName(emailFile));
            var fileExtension = Path.GetExtension(emailFile);

            while (File.Exists(newFileName))
            {
                newFileName = Path.Combine(destinationPath, Path.GetFileNameWithoutExtension(emailFile) + "_" + counter + fileExtension);
                counter++;
            }

            _logger.Info("Moved file to {0}", newFileName);
            
            File.Move(emailFile, newFileName);
        }

        public void Delete(string emailFilePath)
        {
            _logger.Info("Deleted file {0}", emailFilePath);

            File.Delete(emailFilePath);
        }

        public bool Send(MailMessage mailMessage)
        {
            try
            {
                _smtpClient.Send(mailMessage);
                _logger.Info("Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to send email {0}:\n{1}\n{2}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        public string Load(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
