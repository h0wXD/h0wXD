using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface ISendMailBehavior
    {
        void Send(MailMessage message);
    }
}
