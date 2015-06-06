using System.IO;
using System.Net.Mail;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.DataAccess
{
    public class EmailDao : IEmailDao
    {
        private readonly ISendMailBehavior _sendMailBehavior;

        public EmailDao(ISendMailBehavior sendMailBehavior)
        {
            _sendMailBehavior = sendMailBehavior;
        }

        public void SendEmail(MailMessage mailMessage)
        {
            _sendMailBehavior.Send(mailMessage);
        }

        public string LoadEmail(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
