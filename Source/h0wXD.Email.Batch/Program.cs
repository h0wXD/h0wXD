using System;
using h0wXD.Environment.Helpers;
using h0wXD.Email.Batch.Injection;
using h0wXD.Email.Helpers;
using h0wXD.Email.Interfaces;
using h0wXD.Logging.Interfaces;
using Ninject;

namespace h0wXD.Email.Batch
{
    public class Program
    {
        static void Main(string [] _args)
        {
            var logger = ProductionKernel.Instance.Get<ILogger>();
            var emailMessageBuilder = ProductionKernel.Instance.Get<IEmailMessageBuilder>();
            var emailDao = ProductionKernel.Instance.Get<IEmailDao>();

            var sMailFrom = EnvironmentHelper.GetEnvironmentVariable("From", String.Empty);
            var sMailToArray = EnvironmentHelper.GetEnvironmentVariable("To", String.Empty);
            var sMailToCcArray = EnvironmentHelper.GetEnvironmentVariable("ToCc", String.Empty);
            var sMailToBccArray = EnvironmentHelper.GetEnvironmentVariable("ToBcc", String.Empty);
            var sMailSubject = EnvironmentHelper.GetEnvironmentVariable("Subject", String.Empty);
            var sMailContent = EnvironmentHelper.GetEnvironmentVariable("Content", String.Empty);
            var sMailTemplate = EnvironmentHelper.GetEnvironmentVariable("Template", String.Empty);

            try
            {
                if (String.IsNullOrWhiteSpace(sMailFrom) ||
                    (String.IsNullOrWhiteSpace(sMailToArray)
                    && String.IsNullOrWhiteSpace(sMailToCcArray) 
                    && String.IsNullOrWhiteSpace(sMailToBccArray)))
                {
                    throw new ArgumentException("No from / to address specified.");
                }

                emailMessageBuilder.SetSender(sMailFrom);

                foreach (var sMailTo in EmailHelper.SplitMailAddresses(sMailToArray))
                {
                    emailMessageBuilder.AddReceiver(sMailTo);
                }

                foreach (var sMailToCc in EmailHelper.SplitMailAddresses(sMailToCcArray))
                {
                    emailMessageBuilder.AddCcReceiver(sMailToCc);
                }

                foreach (var sMailToBcc in EmailHelper.SplitMailAddresses(sMailToBccArray))
                {
                    emailMessageBuilder.AddBccReceiver(sMailToBcc);
                }

                if (String.IsNullOrWhiteSpace(sMailTemplate))
                {
                    if (String.IsNullOrWhiteSpace(sMailSubject))
                    {
                        throw new ArgumentException("No subject specified.");
                    }

                    emailMessageBuilder.SetSubject(sMailSubject);

                    if (String.IsNullOrWhiteSpace(sMailContent))
                    {
                        throw new ArgumentException("No content specified.");
                    }

                    emailMessageBuilder.AppendBody(sMailContent);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(sMailTemplate))
                    {
                        throw new ArgumentException("No template specified.");
                    }
                
                    emailMessageBuilder.SetTemplate(sMailTemplate);
                }

                emailDao.SendEmail(emailMessageBuilder.ToEmailMessage());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                System.Environment.Exit(1);
            }
        }
    }
}
