using System;
using System.Net.Mail;
using System.Text;
using h0wXD.Email.Helpers;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.Builders
{
    public class EmailMessageBuilder : IEmailMessageBuilder
    {
        private readonly ITemplateManager _templateManager;
        private MailMessage _mailMessage;
        private StringBuilder _contentBuilder;
        private string templatePath;

        public EmailMessageBuilder(ITemplateManager templateManager)
        {
            _templateManager = templateManager;

            Reset();
        }

        public void Reset()
        {
            _mailMessage = new MailMessage();
            _contentBuilder = new StringBuilder(1024);
            templatePath = String.Empty;
        }

        public void SetSender(string mailFromAddress)
        {
            _mailMessage.From = new MailAddress(mailFromAddress);
        }

        public void SetSubject(string subject)
        {
            _mailMessage.Subject = subject;
        }
        
        public void AppendBody(string text)
        {
            if (!String.IsNullOrEmpty(templatePath))
            {
                throw new InvalidOperationException("You cannot append content if a template path is specified.");
            }
            _contentBuilder.Append(text);
        }

        public void SetTemplate(string templatePath)
        {
            if (_contentBuilder.Length > 0)
            {
                throw new InvalidOperationException("You cannot provide a template path after appending to the email body.");
            }
            this.templatePath = templatePath;
        }

        public void AddReceiver(string mailToAddress)
        {
            _mailMessage.To.Add(EmailHelper.ParseMailAddress(mailToAddress));
        }

        public void AddCcReceiver(string mailToAddress)
        {
            _mailMessage.CC.Add(EmailHelper.ParseMailAddress((mailToAddress)));
        }

        public void AddBccReceiver(string mailToAddress)
        {
            _mailMessage.Bcc.Add(EmailHelper.ParseMailAddress((mailToAddress)));
        }
        
        public MailMessage ToEmailMessage()
        {
            if (0 == _contentBuilder.Length &&
                String.IsNullOrEmpty(templatePath))
            {
                throw new InvalidOperationException("Cannot build email without providing a body.");
            }
            
            if (!String.IsNullOrEmpty(templatePath))
            {
                // Apply Template
                //m_templateManager
            }

            _mailMessage.Body = _contentBuilder.ToString();
            _mailMessage.IsBodyHtml = _mailMessage.Body.StartsWith(TechnicalConstants.HtmlTag);

            return _mailMessage;
        }
    }
}
