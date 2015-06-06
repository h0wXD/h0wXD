using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace h0wXD.Email
{
    public static class ExtensionMethods
    {
        public static void Save(this MailMessage mailMessage, string fileName)
        {
            var assembly = typeof(SmtpClient).Assembly;
            var mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            var assemblyVersion = assembly.ToString();

            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { typeof(Stream) }, null);
                var mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
                var sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

                if (assemblyVersion.Contains("Version=4.0.0.0"))
                {
                    // System.Net.Mime.BaseWriter writer, Boolean sendEnvelope, Boolean allowUnicode
                    sendMethod.Invoke(mailMessage, BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { mailWriter, true, true}, null);
                }
                else if (assemblyVersion.Contains("Version=3.5.0.0"))
                {
                    // System.Net.Mime.BaseWriter writer, Boolean sendEnvelope
                    sendMethod.Invoke(mailMessage, BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { mailWriter, true }, null);
                }
                else
                {
                    throw new NotImplementedException("Add hoax code plz!");
                }

                var closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);

                closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
            }
        }
    }
}
