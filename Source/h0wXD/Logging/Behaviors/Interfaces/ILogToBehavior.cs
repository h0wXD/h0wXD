
using System;

namespace h0wXD.Logging.Behaviors.Interfaces
{
    public interface ILogToBehavior
    {
        IMessageFormatBehavior MessageFormatBehavior { get; set; }
        event EventHandler<DayChangedEventArgs> DayChanged;
        void Write(LogEventArgs args);
    }
}
