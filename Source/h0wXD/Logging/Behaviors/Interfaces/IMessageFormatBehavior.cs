using System;

namespace h0wXD.Logging.Behaviors.Interfaces
{
    public interface IMessageFormatBehavior
    {
        string FormatMessage(LogEventArgs args);
        string FormatDate(DateTime date);
        string FormatTime(DateTime time);
    }
}