using System;

namespace h0wXD.Logging.Interfaces
{
    public interface ILogger
    {
        event EventHandler<LogEventArgs> Log;
        event EventHandler<string> LogMessage;

        void Write(string message, params object[] args);
        void Debug(string message, params object [] args);
        void Info(string message, params object [] args);
        void Warning(string message, params object [] args);
        void Error(string message, params object [] args);
        void Fatal(string message, params object [] args);
    }
}
