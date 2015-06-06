using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.Parsers
{
    public class EmailMessageParser : IEmailMessageParser
    {
        private static KeyValuePair<string, string> SplitHeader(string header)
        {
            var headers = header.Split(new [] { ": " }, 1, StringSplitOptions.None);

            return new KeyValuePair<string, string>(headers[0].ToLowerInvariant(), headers.Length > 1 ? headers[1] : "");
        }

        private static string ReadKey(Dictionary<string, string> headers, string key, string defaultValue)
        {
            return headers.ContainsKey(key) ? headers[key] : defaultValue;
        }

        private static string [] ReadKeys(Dictionary<string, string> headers, string key, string defaultValue)
        {
            var value = ReadKey(headers, key, defaultValue);

            if (value == defaultValue)
            {
                return new string [] {};
            }

            return value.Split(new [] {", "}, StringSplitOptions.RemoveEmptyEntries);
        }

        public MailMessage Parse(string emailFileContent)
        {
            var emailMessage = new MailMessage();
            
            using (var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(emailFileContent))))
            {
                var receivers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var headers = ParseHeaders(streamReader);
                var headerMap = headers.Select(SplitHeader).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                emailMessage.Body = ParseBody(streamReader);
                emailMessage.IsBodyHtml = headers.Any(x => x.StartsWith("Content-Type: ") && x.Contains("text/html"));

                emailMessage.Subject = ReadKey(headerMap, "subject", String.Empty);
                emailMessage.From = new MailAddress(ReadKey(headerMap, "from", String.Empty));
                
                foreach (var receiver in ReadKeys(headerMap, "to", String.Empty))
                {
                    if (receivers.Add(receiver))
                    {
                        emailMessage.To.Add(receiver);
                    }
                }

                foreach (var receiver in ReadKeys(headerMap, "cc", String.Empty))
                {
                    if (receivers.Add(receiver))
                    {
                        emailMessage.CC.Add(receiver);
                    }
                }

                foreach (var hiddenReceiver in headers.Where(x => x.StartsWith("X-Receiver: ")).Select(SplitHeader).Select(x => x.Value))
                {
                    if (receivers.Add(hiddenReceiver))
                    {
                        emailMessage.Bcc.Add(new MailAddress(hiddenReceiver));
                    }
                }
            }

            // TODO: Clone Other Headers?

            if (IsInvalid(emailMessage))
            {
                throw new InvalidDataException("The file contains invalid data! Please check the Sender / Receiver List / Subject or Body.");
            }

            return emailMessage;
        }
        
        public IList<string> ParseHeaders(StreamReader reader)
        {
            var headers = new List<string>();
            var areHeadersParsed = false;

            reader.DiscardBufferedData();

            while (!reader.EndOfStream && 
                   !areHeadersParsed)
            {
                var line = reader.ReadLine();

                if (String.IsNullOrEmpty(line))
                {
                    areHeadersParsed = true;
                    continue;
                }

                if (!line.Contains(": ") && 
                    headers.Count > 0)
                {
                    headers[headers.Count - 1] = headers[headers.Count - 1] + line;
                }
                else
                {
                    headers.Add(line);
                }
            }

            if (!areHeadersParsed || 
                reader.EndOfStream)
            {
                throw new InvalidDataException("Could not parse all headers.");   
            }

            return headers;
        }

        public string ParseBody(StreamReader reader)
        {
            var body = new StringBuilder();
            var areHeadersParsed = false;
            
            reader.DiscardBufferedData();
            
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                
                if (String.IsNullOrEmpty(line))
                {
                    areHeadersParsed = true;
                    continue;
                }

                if (areHeadersParsed)
                {
                    var sFixedLine = line[line.Length - 1] == '=' ? line.Remove(line.Length - 1) : line;

                    sFixedLine = sFixedLine.Replace("=0D", "\r").Replace("=0A", "\n");

                    body.Append(sFixedLine);
                }
            }

            if (!areHeadersParsed || 
                reader.EndOfStream)
            {
                throw new InvalidDataException("Could not parse content.");   
            }

            return body.ToString();
        }

        public string ParseHeaderValue(string header)
        {
            var headerLength = header.Length - 1;

            for (var i = 0; i < headerLength; i++)
            {
                if (header[i] == ':' &&
                    header[i + 1] == ' ')
                {
                    return header.Substring(i + 2);
                }
            }

            return header;
        }

        private bool IsInvalid(MailMessage message)
        {
            return ((message.To.Count == 0 &&
                     message.CC.Count == 0 &&
                     message.Bcc.Count == 0)
                     || message.From == null
                     || String.IsNullOrWhiteSpace(message.Subject)
                     || String.IsNullOrWhiteSpace(message.Body));
        }
    }
}
