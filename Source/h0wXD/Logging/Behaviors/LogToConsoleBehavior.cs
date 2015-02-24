using System;
using h0wXD.Logging.Behaviors.Interfaces;

namespace h0wXD.Logging.Behaviors
{
    public class LogToConsoleBehavior : ILogToBehavior
    {
        private readonly string m_sDateFormat;
        private DateTime m_initDate;

        public LogToConsoleBehavior(string _sDateFormat = "")
        {
            m_sDateFormat = _sDateFormat;
            m_initDate = DateTime.Now;
        }

        public void Write(LogEventArgs _args)
        {
            if (m_initDate.DayOfYear !=  _args.Date.DayOfYear)
            {
                m_initDate = DateTime.Now;
                Console.WriteLine(LogMessageFormatter.FormatDate(m_initDate));
            }

            Console.WriteLine(LogMessageFormatter.Format(_args, m_sDateFormat, _args.Date));
        }
    }
}
