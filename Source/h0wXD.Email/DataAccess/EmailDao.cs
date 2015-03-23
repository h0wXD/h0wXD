using System.IO;
using System.Net.Mail;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.DataAccess
{
    public class EmailDao : IEmailDao
    {
        private readonly ISendMailBehavior m_sendMailBehavior;

        public EmailDao(ISendMailBehavior _sendMailBehavior)
        {
            m_sendMailBehavior = _sendMailBehavior;
        }

        public void SendEmail(MailMessage _mailMessage)
        {
            m_sendMailBehavior.Send(_mailMessage);
        }

        public string LoadEmail(string _sFileName)
        {
            return File.ReadAllText(_sFileName);
        }
    }
}
