using System.Text;

namespace h0wXD.IO.Interfaces
{
    public interface ICsvFileReader
    {
        int LineNumber { get; }
        string FileName { get; }
        bool EndOfData { get; }
        bool HasFieldsEnclosedInQuotes { get; set; }
        bool TrimWhiteSpace { get; set; }
        bool SkipFirstLine { get; set; }
        string [] CommentTokens { get; set; }
        string [] Delimiters { get; set; }
        void Reset();
        void Open(string _sFileName, Encoding _encoding);
        void Close();
        string ReadLine();
        string [] ReadFields();
    }
}
