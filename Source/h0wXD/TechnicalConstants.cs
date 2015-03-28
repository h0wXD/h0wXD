
namespace h0wXD
{
    public abstract class TechnicalConstants
    {
        public abstract class Diagnostics
        {
            public const string DoubleQuote = "\"";
            public const int ExitCodeSuccess = 0;
        }

        public abstract class IO
        {
            public const string MaskAny = "*";
            public const string FileMaskAny = "*.*";
            public class CsvFileReader
            {
                public class Exceptions
                {
                    public const string FileNotFound = "Unable to find CSV file.";
                    public const string EndOfData = "End of file has been reached!";
                }
            }
        }

        public abstract class Service
        {
            public const string InstallParameter = "-install";
            public const string UninstallParameter = "-uninstall";
            public const string StartParameter = "-start";
            public const string StopParameter = "-stop";
        }
    }
}
