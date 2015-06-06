using System;
using System.Globalization;
using h0wXD.Logging.Behaviors.Interfaces;
using h0wXD.Logging.Helpers;

namespace h0wXD.Logging.Behaviors
{
    public class MessageFormatBehavior : IMessageFormatBehavior
    {
        private static readonly CultureInfo CultureInfo;

        static MessageFormatBehavior()
        {
            CultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        }

        public string FormatMessage(LogEventArgs args)
        {
            return string.Format("{0}{1}", LogTypeHelper.ToString(args.LogType), args.Message);
        }

        public string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo);
        }

        public string FormatTime(DateTime time)
        {
            return time.ToString("hh:mm:ss tt", CultureInfo);
        }
    }
}
