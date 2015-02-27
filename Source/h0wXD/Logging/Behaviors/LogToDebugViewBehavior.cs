using System;
using System.Diagnostics;
using h0wXD.Logging.Behaviors.Interfaces;

namespace h0wXD.Logging.Behaviors
{
    public class LogToDebugViewBehavior : ILogToBehavior
    {
        private readonly string m_sDateFormat;

        public LogToDebugViewBehavior(string _sDateFormat = "")
        {
            m_sDateFormat = _sDateFormat;
        }

        public void Write(LogEventArgs _args)
        {
            //Trace.WriteLine(LogMessageFormatter.Format(_args, m_sDateFormat, _args.Date));
            Debug.WriteLine(LogMessageFormatter.Format(_args, m_sDateFormat, _args.Date));
        }
    }
}
