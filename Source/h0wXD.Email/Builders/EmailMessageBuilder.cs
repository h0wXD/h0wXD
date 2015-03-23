using System;
using System.Net.Mail;
using System.Text;
using h0wXD.Email.Helpers;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.Builders
{
    public class EmailMessageBuilder : IEmailMessageBuilder
    {
        private readonly ITemplateManager m_templateManager;
        private MailMessage m_mailMessage;
        private StringBuilder m_contentBuilder;
        private string m_sTemplatePath;

        public EmailMessageBuilder(ITemplateManager _templateManager)
        {
            m_templateManager = _templateManager;

            Reset();
        }

        public void Reset()
        {
            m_mailMessage = new MailMessage();
            m_contentBuilder = new StringBuilder(1024);
            m_sTemplatePath = String.Empty;
        }

        public void SetSender(string _sMailFromAddress)
        {
            m_mailMessage.From = new MailAddress(_sMailFromAddress);
        }

        public void SetSubject(string _sMailSubject)
        {
            m_mailMessage.Subject = _sMailSubject;
        }
        
        public void AppendBody(string _sMailBodyContent)
        {
            if (!String.IsNullOrEmpty(m_sTemplatePath))
            {
                throw new InvalidOperationException("You cannot append content if a template path is specified.");
            }
            m_contentBuilder.Append(_sMailBodyContent);
        }

        public void SetTemplate(string _sTemplatePath)
        {
            if (m_contentBuilder.Length > 0)
            {
                throw new InvalidOperationException("You cannot provide a template path after appending to the email body.");
            }
            m_sTemplatePath = _sTemplatePath;
        }

        public void AddReceiver(string _sMailToAddress)
        {
            m_mailMessage.To.Add(EmailHelper.ParseMailAddress(_sMailToAddress));
        }

        public void AddCcReceiver(string _sMailToAddress)
        {
            m_mailMessage.CC.Add(EmailHelper.ParseMailAddress((_sMailToAddress)));
        }

        public void AddBccReceiver(string _sMailToAddress)
        {
            m_mailMessage.Bcc.Add(EmailHelper.ParseMailAddress((_sMailToAddress)));
        }
        
        public MailMessage ToEmailMessage()
        {
            if (0 == m_contentBuilder.Length &&
                String.IsNullOrEmpty(m_sTemplatePath))
            {
                throw new InvalidOperationException("Cannot build email without providing a body.");
            }
            
            if (!String.IsNullOrEmpty(m_sTemplatePath))
            {
                // Apply Template
                //m_templateManager
            }

            m_mailMessage.Body = m_contentBuilder.ToString();
            m_mailMessage.IsBodyHtml = m_mailMessage.Body.StartsWith(TechnicalConstants.HtmlTag);

            return m_mailMessage;
        }
    }
}
