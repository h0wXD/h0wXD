using System;
using h0wXD.Logging.Behaviors.Interfaces;

namespace h0wXD.Logging.Behaviors
{
    public class LogToConsoleBehavior : ILogToBehavior
    {
        private DateTime _initDate;

        public IMessageFormatBehavior MessageFormatBehavior { get; set; }
        public event EventHandler<DayChangedEventArgs> DayChanged = delegate { };

        public LogToConsoleBehavior()
            : this(new MessageFormatBehavior())
        {
        }

        public LogToConsoleBehavior(IMessageFormatBehavior messageFormatBehavior)
        {
            MessageFormatBehavior = messageFormatBehavior;
            _initDate = DateTime.Now;
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
                    Console.WriteLine(eventArgs.Message);
                }
            }

            Console.WriteLine(MessageFormatBehavior.FormatMessage(args));
        }
    }
}
