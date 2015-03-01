
namespace h0wXD
{
    public class TechnicalConstants
    {
        public class IO
        {
            public class CsvFileReader
            {
                public class Exceptions
                {
                    public const string FileNotFound = "Unable to find CSV file.";
                    public const string EndOfData = "End of file has been reached!";
                }
            }
        }

        public class Service
        {
            public const string InstallParameter = "-install";
            public const string UninstallParameter = "-uninstall";
            public const string StartParameter = "-start";
            public const string StopParameter = "-stop";
        }
    }
}
