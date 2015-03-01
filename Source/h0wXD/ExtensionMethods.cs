using System;
using System.Net;
using System.Text.RegularExpressions;

namespace h0wXD
{
    public static class ExtensionMethods
    {
        private static Regex ms_endPointRegex = new Regex(@"^(?<ip>(?:\[[\da-fA-F:]+\])|(?:\d{1,3}\.){3}\d{1,3}):(?<port>\d+)?$", RegexOptions.Compiled);
        public static IPEndPoint ToIPEndPoint(this string _sEndPoint)
        {
            var match = ms_endPointRegex.Match(_sEndPoint);
            
            if (!match.Success)
            {
                throw new ArgumentException("The string could not be parsed to an EndPoint.");
            }

            return new IPEndPoint(IPAddress.Parse(match.Groups["ip"].Value), int.Parse(match.Groups["port"].Value));
        }

        public static bool StartsWith(this string _sText, string _sOtherText, int _iMinimumMatchCount, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            _iMinimumMatchCount = Math.Min(_sText.Length, _iMinimumMatchCount);

            var sTextToMatch = _sOtherText.Substring(0, Math.Min(_sOtherText.Length, _iMinimumMatchCount));

            if (_sOtherText.Length == _sText.Length)
            {
                return _sOtherText.Equals(_sText, _comparisonType);
            }

            return _sText.StartsWith(sTextToMatch, _comparisonType);
        }
    }
}
