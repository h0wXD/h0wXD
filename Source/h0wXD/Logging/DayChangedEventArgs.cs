using System;

namespace h0wXD.Logging
{
    public class DayChangedEventArgs : EventArgs
    {
        public DateTime Previous { get; set; }
        public DateTime Next { get; set; }
        public string Message { get; set; }
        public object Tag { get; set; }
    }
}
