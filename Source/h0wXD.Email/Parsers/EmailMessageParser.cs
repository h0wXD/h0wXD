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
        private static KeyValuePair<string, string> SplitHeader(string _sHeader)
        {
            var sHeaderArray = _sHeader.Split(new [] { ": " }, 1, StringSplitOptions.None);

            return new KeyValuePair<string, string>(sHeaderArray[0].ToLowerInvariant(), sHeaderArray.Length > 1 ? sHeaderArray[1] : "");
        }

        private static string ReadKey(Dictionary<string, string> _headerMap, string _sKey, string _defaultValue)
        {
            return _headerMap.ContainsKey(_sKey) ? _headerMap[_sKey] : _defaultValue;
        }

        private static string [] ReadKeys(Dictionary<string, string> _headerMap, string _sKey, string _defaultValue)
        {
            var sValue = ReadKey(_headerMap, _sKey, _defaultValue);

            if (sValue == _defaultValue)
            {
                return new string [] {};
            }

            return sValue.Split(new [] {", "}, StringSplitOptions.RemoveEmptyEntries);
        }

        public MailMessage Parse(string _sEmailFileContent)
        {
            var emailMessage = new MailMessage();
            
            using (var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(_sEmailFileContent))))
            {
                var sReceiverList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var sHeaderList = ParseHeaders(streamReader);
                var sHeaderMap = sHeaderList.Select(SplitHeader).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                emailMessage.Body = ParseBody(streamReader);
                emailMessage.IsBodyHtml = sHeaderList.Any(x => x.StartsWith("Content-Type: ") && x.Contains("text/html"));

                emailMessage.Subject = ReadKey(sHeaderMap, "subject", String.Empty);
                emailMessage.From = new MailAddress(ReadKey(sHeaderMap, "from", String.Empty));
                
                foreach (var sReceiver in ReadKeys(sHeaderMap, "to", String.Empty))
                {
                    if (sReceiverList.Add(sReceiver))
                    {
                        emailMessage.To.Add(sReceiver);
                    }
                }

                foreach (var sReceiver in ReadKeys(sHeaderMap, "cc", String.Empty))
                {
                    if (sReceiverList.Add(sReceiver))
                    {
                        emailMessage.CC.Add(sReceiver);
                    }
                }

                foreach (var sHiddenReceiver in sHeaderList.Where(x => x.StartsWith("X-Receiver: ")).Select(SplitHeader).Select(x => x.Value))
                {
                    if (sReceiverList.Add(sHiddenReceiver))
                    {
                        emailMessage.Bcc.Add(new MailAddress(sHiddenReceiver));
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
        
        public IList<string> ParseHeaders(StreamReader _reader)
        {
            var sHeaderList = new List<string>();
            var bAreHeadersParsed = false;

            _reader.DiscardBufferedData();

            while (!_reader.EndOfStream && 
                   !bAreHeadersParsed)
            {
                var sLine = _reader.ReadLine();

                if (String.IsNullOrEmpty(sLine))
                {
                    bAreHeadersParsed = true;
                    continue;
                }

                if (!sLine.Contains(": ") && 
                    sHeaderList.Count > 0)
                {
                    sHeaderList[sHeaderList.Count - 1] = sHeaderList[sHeaderList.Count - 1] + sLine;
                }
                else
                {
                    sHeaderList.Add(sLine);
                }
            }

            if (!bAreHeadersParsed || 
                _reader.EndOfStream)
            {
                throw new InvalidDataException("Could not parse all headers.");   
            }

            return sHeaderList;
        }

        public string ParseBody(StreamReader _reader)
        {
            var sBody = new StringBuilder();
            var bAreHeadersParsed = false;
            
            _reader.DiscardBufferedData();
            
            while (!_reader.EndOfStream)
            {
                var sLine = _reader.ReadLine();
                
                if (String.IsNullOrEmpty(sLine))
                {
                    bAreHeadersParsed = true;
                    continue;
                }

                if (bAreHeadersParsed)
                {
                    var sFixedLine = sLine[sLine.Length - 1] == '=' ? sLine.Remove(sLine.Length - 1) : sLine;

                    sFixedLine = sFixedLine.Replace("=0D", "\r").Replace("=0A", "\n");

                    sBody.Append(sFixedLine);
                }
            }

            if (!bAreHeadersParsed || 
                _reader.EndOfStream)
            {
                throw new InvalidDataException("Could not parse content.");   
            }

            return sBody.ToString();
        }

        public string ParseHeaderValue(string _sHeader)
        {
            var iHeaderLength = _sHeader.Length - 1;

            for (var i = 0; i < iHeaderLength; i++)
            {
                if (_sHeader[i] == ':' &&
                    _sHeader[i + 1] == ' ')
                {
                    return _sHeader.Substring(i + 2);
                }
            }

            return _sHeader;
        }

        private bool IsInvalid(MailMessage _message)
        {
            return ((_message.To.Count == 0 &&
                     _message.CC.Count == 0 &&
                     _message.Bcc.Count == 0)
                     || _message.From == null
                     || String.IsNullOrWhiteSpace(_message.Subject)
                     || String.IsNullOrWhiteSpace(_message.Body));
        }
    }
}
