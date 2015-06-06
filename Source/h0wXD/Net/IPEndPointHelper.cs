using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace h0wXD.Net
{
    public static class IPEndPointHelper
    {
        private static readonly Regex EndPointRegex = new Regex(@"^(?<ip>(?:\[[\da-fA-F:]+\])|(?:\d{1,3}\.){3}\d{1,3}):(?<port>\d+)?$", RegexOptions.Compiled);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IPEndPoint ToIPEndPoint(this string endPoint)
        {
            var match = EndPointRegex.Match(endPoint);
            
            if (!match.Success)
            {
                throw new ArgumentException("The string could not be parsed to an EndPoint.");
            }

            return new IPEndPoint(IPAddress.Parse(match.Groups["ip"].Value), int.Parse(match.Groups["port"].Value));
        }
    }
}
