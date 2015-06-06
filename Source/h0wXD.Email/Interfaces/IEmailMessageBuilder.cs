using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface IEmailMessageBuilder
    {
        void Reset();
        void SetSender(string mailFromAddress);
        void SetSubject(string subject);
        void AppendBody(string text);
        void SetTemplate(string templatePath);
        void AddReceiver(string mailToAddress);
        void AddCcReceiver(string mailToAddress);
        void AddBccReceiver(string mailToAddress);
        MailMessage ToEmailMessage();
    }
}
