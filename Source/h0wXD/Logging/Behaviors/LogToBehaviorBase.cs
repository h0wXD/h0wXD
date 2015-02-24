
namespace h0wXD.Logging.Behaviors
{
    public abstract class BehaviorBase
    {
        protected LogMessageFormatter m_logMessageFormatter;

        protected BehaviorBase()
        {
            m_logMessageFormatter = new LogMessageFormatter();
        }

        public abstract void Write(LogEventArgs _args);
    }
}
