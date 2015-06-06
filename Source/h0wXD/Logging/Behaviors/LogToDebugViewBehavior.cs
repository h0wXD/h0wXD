using System;
using System.Diagnostics;
using h0wXD.Logging.Behaviors.Interfaces;

namespace h0wXD.Logging.Behaviors
{
    public class LogToDebugViewBehavior : ILogToBehavior
    {
        private DateTime _initDate;

        public IMessageFormatBehavior MessageFormatBehavior { get; set; }
        public event EventHandler<DayChangedEventArgs> DayChanged = delegate { };

        public LogToDebugViewBehavior(IMessageFormatBehavior messageFormatBehavior)
        {
            _initDate = DateTime.Now;
            MessageFormatBehavior = messageFormatBehavior;
        }

        public void Write(LogEventArgs args)
        {
            if (_initDate.DayOfYear != args.Date.DayOfYear)
            {
                var eventArgs = new DayChangedEventArgs()
                {
                    Previous =_initDate,
                    Next = args.Date
                };

                _initDate = args.Date;

                DayChanged(this, eventArgs);

                if (!string.IsNullOrWhiteSpace(eventArgs.Message))
                {
                    Console.WriteLine(eventArgs.Message);
                }
            }

            //Trace.WriteLine(MessageFormatBehavior.FormatMessage(args));
            Debug.WriteLine(MessageFormatBehavior.FormatMessage(args));
        }
    }
}
