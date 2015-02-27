using h0wXD.Logging;
using h0wXD.Logging.Behaviors;

namespace h0wXD.Test.Logging
{
    public class TestLogger : Logger
    {
        public LogToFileBehavior LogToFileBehavior { get; private set; }
        public LogToFileBehavior LogToFileBehaviorSwapDaily { get; private set; }

        public TestLogger()
        {
            AddBehavior(new LogToDebugViewBehavior());
            AddBehavior(new LogToConsoleBehavior());
            AddBehavior(LogToFileBehavior = new LogToFileBehavior());
            AddBehavior(LogToFileBehaviorSwapDaily = new LogToFileBehavior(@".\Log", "hh:mm:ss tt", true));
        }
    }
}
