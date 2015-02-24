using System;

namespace h0wXD.Logging.Interfaces
{
    public interface ILogger
    {
        string LastMessage { get; }

        event EventHandler<LogEventArgs> Log;
        event EventHandler<string> LogMessage;

        void Write(string _sMessage, params object [] _args);
        void Debug(string _sMessage, params object [] _args);
        void Info(string _sMessage, params object [] _args);
        void Warning(string _sMessage, params object [] _args);
        void Error(string _sMessage, params object [] _args);
        void Fatal(string _sMessage, params object [] _args);
    }
}
