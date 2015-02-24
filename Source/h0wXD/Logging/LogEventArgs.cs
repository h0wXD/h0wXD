using System;

namespace h0wXD.Logging
{
    public class LogEventArgs : EventArgs
    {
        public LogType LogType { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
