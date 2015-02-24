using System;
using System.Globalization;

namespace h0wXD.Logging
{
    public class LogMessageFormatter
    {
        private static readonly CultureInfo ms_cultureInfo;

        static LogMessageFormatter()
        {
            ms_cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        }

        public static string Format(LogEventArgs _args, string _sDateFormat, DateTime ? _currentDate)
        {
            if (!String.IsNullOrEmpty(_sDateFormat) && _currentDate.HasValue)
            {
                return String.Format("[{0}] {1}{2}", _currentDate.Value.ToString(_sDateFormat, ms_cultureInfo), GetLogTypeString(_args.LogType), _args.Message);
            }

            return String.Format("{0}{1}", GetLogTypeString(_args.LogType), _args.Message);
        }

        public static string FormatDate(DateTime _date)
        {
            return _date.ToString("yyyy-MM-dd", ms_cultureInfo);
        }

        public static string FormatTime(DateTime _date)
        {
            return _date.ToString("hh:mm:ss tt", ms_cultureInfo);
        }

        private static string GetLogTypeString(LogType _logType)
        {
            switch (_logType)
            {
                case LogType.Debug: return "[DEBUG] ";
                case LogType.Info: return "[INFO] ";
                case LogType.Warning: return "[WARN] ";
                case LogType.Error: return "[ERROR] ";
                case LogType.Fatal: return "[FATAL] ";
                default: return "";
            }
        }
    }
}
