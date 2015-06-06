
namespace h0wXD.Logging.Helpers
{
    public static class LogTypeHelper
    {
        public static string ToString(LogType logType)
        {
            switch (logType)
            {
                case LogType.Debug: return "[DEBUG] ";
                case LogType.Info: return "[INFO] ";
                case LogType.Warning: return "[WARN] ";
                case LogType.Error: return "[ERROR] ";
                case LogType.Fatal: return "[FATAL] ";
            }

            return null;
        } 
    }
}