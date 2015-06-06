using System.Text.RegularExpressions;

namespace h0wXD.Collections.Elements.Interfaces
{
    public interface IRegexFunctionElement
    {
        Regex Regex { get; }
        int Count { get; }
        string [] Keys { get; }
        int [] Indices { get; }
        string Value { get; }
        void Match(string content);
        string this[int index] { get; }
        string this[string key] { get; }
    }
}