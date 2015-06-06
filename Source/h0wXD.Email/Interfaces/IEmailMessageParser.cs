using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface IEmailMessageParser
    {
        MailMessage Parse(string emailFileContent);
        IList<string> ParseHeaders(StreamReader reader);
        string ParseBody(StreamReader reader);
        string ParseHeaderValue(string header);
    }
}
