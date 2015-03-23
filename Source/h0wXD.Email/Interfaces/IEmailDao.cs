using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface IEmailDao
    {
        void SendEmail(MailMessage _mailMessage);

        string LoadEmail(string _sFileName);
    }
}
