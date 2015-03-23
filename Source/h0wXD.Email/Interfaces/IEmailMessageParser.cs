using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace h0wXD.Email.Interfaces
{
    public interface IEmailMessageParser
    {
        MailMessage Parse(string _sEmailFileContent);
        IList<string> ParseHeaders(StreamReader _reader);
        string ParseBody(StreamReader _reader);
        string ParseHeaderValue(string _sHeader);
    }
}
