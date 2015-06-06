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

            var mailFromAddress = EnvironmentHelper.GetEnvironmentVariable("From", String.Empty);
            var mailToAddresses = EnvironmentHelper.GetEnvironmentVariable("To", String.Empty);
            var mailToCcAddresses = EnvironmentHelper.GetEnvironmentVariable("ToCc", String.Empty);
            var mailToBccAddresses = EnvironmentHelper.GetEnvironmentVariable("ToBcc", String.Empty);
            var mailSubject = EnvironmentHelper.GetEnvironmentVariable("Subject", String.Empty);
            var mailContent = EnvironmentHelper.GetEnvironmentVariable("Content", String.Empty);
            var mailTemplate = EnvironmentHelper.GetEnvironmentVariable("Template", String.Empty);

            try
            {
                if (String.IsNullOrWhiteSpace(mailFromAddress) ||
                    (String.IsNullOrWhiteSpace(mailToAddresses)
                    && String.IsNullOrWhiteSpace(mailToCcAddresses) 
                    && String.IsNullOrWhiteSpace(mailToBccAddresses)))
                {
                    throw new ArgumentException("No from / to address specified.");
                }

                emailMessageBuilder.SetSender(mailFromAddress);

                foreach (var mailToAddress in EmailHelper.SplitMailAddresses(mailToAddresses))
                {
                    emailMessageBuilder.AddReceiver(mailToAddress);
                }

                foreach (var mailToCcAddress in EmailHelper.SplitMailAddresses(mailToCcAddresses))
                {
                    emailMessageBuilder.AddCcReceiver(mailToCcAddress);
                }

                foreach (var mailToBccAddress in EmailHelper.SplitMailAddresses(mailToBccAddresses))
                {
                    emailMessageBuilder.AddBccReceiver(mailToBccAddress);
                }

                if (String.IsNullOrWhiteSpace(mailTemplate))
                {
                    if (String.IsNullOrWhiteSpace(mailSubject))
                    {
                        throw new ArgumentException("No subject specified.");
                    }

                    emailMessageBuilder.SetSubject(mailSubject);

                    if (String.IsNullOrWhiteSpace(mailContent))
                    {
                        throw new ArgumentException("No content specified.");
                    }

                    emailMessageBuilder.AppendBody(mailContent);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(mailTemplate))
                    {
                        throw new ArgumentException("No template specified.");
                    }
                
                    emailMessageBuilder.SetTemplate(mailTemplate);
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
