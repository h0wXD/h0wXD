using System;
using System.Collections.Generic;
using System.Diagnostics;
using h0wXD.Logging.Behaviors.Interfaces;
using h0wXD.Logging.Interfaces;

namespace h0wXD.Logging
{
    public class Logger : ILogger
    {
        private readonly List<ILogToBehavior> m_behaviorList;
        public string LastMessage { get; private set; }

        public event EventHandler<LogEventArgs> Log;
        public event EventHandler<string> LogMessage;
        
        public Logger()
        {
            m_behaviorList = new List<ILogToBehavior>();
        }

        public void AddBehavior(ILogToBehavior _behavior)
        {
            m_behaviorList.Add(_behavior);
        }

        public void Write(string _sMessage, params object [] _args)
        {
            InternalWrite(LogType.Normal, _sMessage, _args);
        }

        //[Conditional("DEBUG"), DebuggerStepThrough]
        public void Debug(string _sMessage, params object [] _args)
        {
#if DEBUG
            InternalWrite(LogType.Debug, _sMessage, _args);
#endif
        }

        public void Info(string _sMessage, params object [] _args)
        {
            InternalWrite(LogType.Info, _sMessage, _args);
        }

        public void Warning(string _sMessage, params object [] _args)
        {
            InternalWrite(LogType.Warning, _sMessage, _args);
        }

        public void Error(string _sMessage, params object [] _args)
        {
            InternalWrite(LogType.Error, _sMessage, _args);
        }

        public void Fatal(string _sMessage, params object [] _args)
        {
            InternalWrite(LogType.Fatal, _sMessage, _args);
        }

        private void InternalWrite(LogType _logType, string _sMessage, params object[] _args)
        {
            var logEventArgs = new LogEventArgs()
            {
                LogType = _logType,
                Date = DateTime.Now,
                Message = String.Format(_sMessage, _args)
            };

            LastMessage = logEventArgs.Message;

            if (Log != null)
            {
                Log(this, logEventArgs);
            }

            if (LogMessage != null)
            {
                LogMessage(this, logEventArgs.Message);
            }

            foreach (var logBehavior in m_behaviorList)
            {
                logBehavior.Write(logEventArgs);
            }
        }
    }
}
