using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using h0wXD.Collections.Elements;
using h0wXD.Collections.Elements.Interfaces;
using h0wXD.Collections.Interfaces;

namespace h0wXD.Collections
{
    public class RegexFunctionMap<T> : IRegexFunctionMap<T>
    {
        private readonly Dictionary<T, IRegexFunctionElement> _regexFunctionElements;

        public RegexFunctionMap()
        {
            _regexFunctionElements = new Dictionary<T, IRegexFunctionElement>();
        }

        public void Add(T key, IRegexFunctionElement element)
        {
            _regexFunctionElements[key] = element;
        }

        public void Add(T key, string pattern, RegexOptions options = RegexOptions.None)
        {
            _regexFunctionElements[key] = new RegexFunctionElement(pattern, options);
        }

        public void Match(string content)
        {
            foreach (var keypair in _regexFunctionElements)
            {
                keypair.Value.Match(content);
            }
        }

        public void Clear()
        {
            _regexFunctionElements.Clear();
        }

        public string this[T key]
        {
            get
            {
                CheckForKey(key);
                return _regexFunctionElements[key].Value;
            }
        }

        public IRegexFunctionElement At(T key)
        {
            CheckForKey(key);
            return _regexFunctionElements[key];
        }

        public IEnumerator<KeyValuePair<T, IRegexFunctionElement>> GetEnumerator()
        {
            return _regexFunctionElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CheckForKey(T key)
        {
            if (!_regexFunctionElements.ContainsKey(key))
            {
                throw new KeyNotFoundException("Couldn't find element " + key + " in collection.");
            }
        }
    }
}
