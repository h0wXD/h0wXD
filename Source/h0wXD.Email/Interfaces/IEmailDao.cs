using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface IEmailDao
    {
        void SendEmail(MailMessage mailMessage);

        string LoadEmail(string fileName);
    }
}
