using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace h0wXD.Email.Helpers
{
    public static class EmailHelper
    {
        public static string [] SplitMailAddresses(string _sMultipleMailAddresses)
        {
            return _sMultipleMailAddresses.Split(';');
        }

        public static MailAddress ParseMailAddress(string _sMailAddress)
        {
            try
            {
                return new MailAddress(_sMailAddress);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not parse email address " + _sMailAddress, ex);
            }
        }
    }
}
