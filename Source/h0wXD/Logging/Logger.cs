using System;
using System.Collections.Generic;
using h0wXD.Logging.Behaviors.Interfaces;
using h0wXD.Logging.Interfaces;

namespace h0wXD.Logging
{
    public class Logger : ILogger
    {
        private readonly List<ILogToBehavior> _logBehaviours;

        public event EventHandler<LogEventArgs> Log = delegate { };
        public event EventHandler<string> LogMessage = delegate { };
        
        public Logger()
        {
            _logBehaviours = new List<ILogToBehavior>();
        }

        public void AddBehavior(ILogToBehavior behavior)
        {
            _logBehaviours.Add(behavior);
        }

        public void Write(string message, params object [] args)
        {
            InternalWrite(LogType.Normal, message, args);
        }

        //[Conditional("DEBUG"), DebuggerStepThrough]
        public void Debug(string message, params object [] args)
        {
#if DEBUG
            InternalWrite(LogType.Debug, message, args);
#endif
        }

        public void Info(string message, params object [] args)
        {
            InternalWrite(LogType.Info, message, args);
        }

        public void Warning(string message, params object [] args)
        {
            InternalWrite(LogType.Warning, message, args);
        }

        public void Error(string message, params object [] args)
        {
            InternalWrite(LogType.Error, message, args);
        }

        public void Fatal(string message, params object [] args)
        {
            InternalWrite(LogType.Fatal, message, args);
        }

        private void InternalWrite(LogType logType, string message, params object [] args)
        {
            var logEventArgs = new LogEventArgs()
            {
                LogType = logType,
                Date = DateTime.Now,
                Message = string.Format(message, args)
            };

            Log(this, logEventArgs);
            LogMessage(this, logEventArgs.Message);

            foreach (var logBehavior in _logBehaviours)
            {
                logBehavior.Write(logEventArgs);
            }
        }
    }
}
