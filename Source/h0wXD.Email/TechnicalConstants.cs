
namespace h0wXD.Email
{
    public class TechnicalConstants
    {
        public const string DefaultErrorSubject = "Error Occured!";

        public const string HtmlTag = "<html>";
        
        public class Templates
        {
            public const string Error = "error.html";
            public const string HtmlFileMask = "*.html";
            
            public enum ReverseKeys : int
            {
                Title,
            }
            
            public class Regex
            {
                public const string Title = "<title>(.*?)</title>";
            }
        }
    }
}
