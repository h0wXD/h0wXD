using System;
using System.IO;
using h0wXD.Logging.Behaviors.Interfaces;

namespace h0wXD.Logging.Behaviors
{
    public class LogToFileBehavior : ILogToBehavior
    {
        private readonly string m_sDateFormat;
        private readonly string m_sLogPath;
        private readonly bool m_bSwapDaily;
        private StreamWriter m_streamWriter;
        private DateTime m_initDate;

        public string CurrentFile { get; private set; }

        public LogToFileBehavior(string _sLogPath = @".\Log", string _sDateFormat = "hh:mm:ss tt", bool _bSwapDaily = false)
        {
            m_sLogPath = _sLogPath;
            m_sDateFormat = _sDateFormat;
            m_bSwapDaily = _bSwapDaily;
            
            if (!Directory.Exists(m_sLogPath))
            {
                Directory.CreateDirectory(m_sLogPath);
            }
            
            m_initDate = DateTime.Now;

            InitStreamWriter();
        }

        public void Write(LogEventArgs _args)
        {
            if (m_initDate.DayOfYear !=  _args.Date.DayOfYear)
            {
                m_initDate = _args.Date;

                if (m_bSwapDaily)
                {
                    InitStreamWriter();
                }
            }

            m_streamWriter.WriteLine(LogMessageFormatter.Format(_args, m_sDateFormat, _args.Date));
            m_streamWriter.Flush();
        }
        
        private void InitStreamWriter()
        {
            if (m_streamWriter != null)
            {
                Dispose();
            }

            CurrentFile = Path.Combine(m_sLogPath, m_initDate.ToString(m_bSwapDaily ? "yyyy-MM-dd" : "yyyy-MM-dd_HH-mm-ss")) + ".txt";

            m_streamWriter = new StreamWriter(CurrentFile, true);

            m_streamWriter.WriteLine(LogMessageFormatter.FormatDate(m_initDate));

            Write(new LogEventArgs()
            {
                LogType = LogType.Normal,
                Date = m_initDate,
                Message = String.Format("Started logging at {0}.", CurrentFile)
            });
        }
        
        public void Dispose()
        {
            m_streamWriter.Flush();
            m_streamWriter.Close();
            m_streamWriter.Dispose();
        }
    }
}
