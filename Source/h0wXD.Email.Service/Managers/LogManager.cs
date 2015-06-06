using System;
using System.IO;
using h0wXD.Logging;
using h0wXD.Logging.Behaviors;

namespace h0wXD.Email.Service.Managers
{
    public class LogManager : Logger
    {
        private readonly LogToFileBehavior _logToFileBehavior;

        public LogManager()
        {
            _logToFileBehavior = new LogToFileBehavior();
            _logToFileBehavior.DayChanged += LogToFileBehaviorOnDayChanged;

            AddBehavior(_logToFileBehavior);
        }

        private void LogToFileBehaviorOnDayChanged(object sender, DayChangedEventArgs e)
        {
            var logPath = Path.GetDirectoryName(_logToFileBehavior.CurrentFile);
            var newLogFile = Path.Combine(logPath, e.Next.ToString("yyyy-MM-dd_HH-mm-ss")) + ".txt";

            e.Message = "[" + _logToFileBehavior.MessageFormatBehavior.FormatDate(e.Next) + "]";
            e.Tag = _logToFileBehavior.OpenFile(newLogFile);
        }
    }
}
