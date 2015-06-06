using System;
using System.IO;
using h0wXD.Logging.Behaviors.Interfaces;

namespace h0wXD.Logging.Behaviors
{
    public class LogToFileBehavior : ILogToBehavior
    {
        private readonly string _logPath;
        private StreamWriter _streamWriter;
        private DateTime _initDate;

        public string CurrentFile { get; private set; }
        public IMessageFormatBehavior MessageFormatBehavior { get; set; }
        public event EventHandler<DayChangedEventArgs> DayChanged = delegate { };

        public LogToFileBehavior(IMessageFormatBehavior messageFormatBehavior, string logPath = @".\Log")
            : this(null, messageFormatBehavior, logPath)
        {
        }

        public LogToFileBehavior(StreamWriter writer, IMessageFormatBehavior messageFormatBehavior, string logPath = @".\Log")
        {
            MessageFormatBehavior = messageFormatBehavior;
            _logPath = logPath;
            _initDate = DateTime.Now;

            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }

            StartLogging(writer ?? OpenFile(Path.Combine(_logPath, _initDate.ToString("yyyy-MM-dd_HH-mm-ss")) + ".txt"));
        }

        public StreamWriter OpenFile(string filePath)
        {
            CurrentFile = filePath;
            return new StreamWriter(CurrentFile, true);
        }

        public void Write(LogEventArgs args)
        {
            if (_initDate.DayOfYear !=  args.Date.DayOfYear)
            {
                var eventArgs = new DayChangedEventArgs()
                {
                    Previous = _initDate,
                    Next = args.Date
                };

                _initDate = args.Date;

                DayChanged(this, eventArgs);

                if (!string.IsNullOrWhiteSpace(eventArgs.Message))
                {
                    _streamWriter.WriteLine(eventArgs.Message);
                }

                if (eventArgs.Tag != null)
                {
                    StartLogging((StreamWriter)eventArgs.Tag);
                }
            }

            _streamWriter.WriteLine(MessageFormatBehavior.FormatMessage(args));
            _streamWriter.Flush();
        }

        public void Close()
        {
            if (_streamWriter != null)
            {
                Dispose();
            }
        }
        
        private void StartLogging(StreamWriter writer)
        {
            Close();

            _streamWriter = writer;

            Write(new LogEventArgs()
            {
                LogType = LogType.Normal,
                Date = _initDate,
                Message = string.Format("Started logging at {0}.", CurrentFile)
            });
        }
        
        public void Dispose()
        {
            _streamWriter.Flush();
            _streamWriter.Close();
            _streamWriter.Dispose();
            _streamWriter = null;
        }
    }
}
