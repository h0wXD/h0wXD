using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface IEmailMessageBuilder
    {
        void Reset();
        void SetSender(string _sMailFromAddress);
        void SetSubject(string _sMailSubject);
        void AppendBody(string _sMailBodyContent);
        void SetTemplate(string _sTemplatePath);
        void AddReceiver(string _sMailToAddress);
        void AddCcReceiver(string _sMailToAddress);
        void AddBccReceiver(string _sMailToAddress);
        MailMessage ToEmailMessage();
    }
}
