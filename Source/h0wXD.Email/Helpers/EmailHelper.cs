using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace h0wXD.Email.Helpers
{
    public static class EmailHelper
    {
        public static string [] SplitMailAddresses(string multipleMailAddresses)
        {
            return multipleMailAddresses.Split(';');
        }

        public static MailAddress ParseMailAddress(string mailAddress)
        {
            try
            {
                return new MailAddress(mailAddress);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not parse email address " + mailAddress, ex);
            }
        }
    }
}
