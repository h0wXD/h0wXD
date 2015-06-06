using System;
using System.IO;
using System.Net.Mail;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.Behaviors
{
    public class DropEmailBehavior : ISendMailBehavior
    {
        private readonly IDropEmailConfiguration _dropEmailConfiguration;

        public DropEmailBehavior(IDropEmailConfiguration dropEmailConfiguration)
        {
            _dropEmailConfiguration = dropEmailConfiguration;
        }

        public void Send(MailMessage message)
        {
            var fileName = Path.Combine(_dropEmailConfiguration.DropFolder, Guid.NewGuid().ToString()) + ".eml";
            message.Save(fileName);
        }
    }
}
