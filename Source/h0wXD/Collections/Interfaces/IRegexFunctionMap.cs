using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using h0wXD.Collections.Elements.Interfaces;

namespace h0wXD.Collections.Interfaces
{
    public interface IRegexFunctionMap<T> : IEnumerable<KeyValuePair<T, IRegexFunctionElement>>, IEnumerable
    {
        void Add(T key, IRegexFunctionElement element);
        void Add(T key, string pattern, RegexOptions options = RegexOptions.None);
        void Match(string content);
        void Clear();
        string this[T key] { get; }
        IRegexFunctionElement At(T key);
    }
}