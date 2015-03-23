using System;
using System.IO;
using System.Net.Mail;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.Behaviors
{
    public class DropEmailBehavior : ISendMailBehavior
    {
        private readonly IDropEmailConfiguration m_dropEmailConfiguration;

        public DropEmailBehavior(IDropEmailConfiguration _dropEmailConfiguration)
        {
            m_dropEmailConfiguration = _dropEmailConfiguration;
        }

        public void Send(MailMessage _message)
        {
            var sFileName = Path.Combine(m_dropEmailConfiguration.DropFolder, Guid.NewGuid().ToString()) + ".eml";
            _message.Save(sFileName);
        }
    }
}
