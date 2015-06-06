using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using h0wXD.Collections.Elements.Interfaces;

namespace h0wXD.Collections.Elements
{
    public class RegexFunctionElement : IRegexFunctionElement
    {
        private readonly string _defaultGroupValueKey;
        private readonly int _defaultGroupValueIndex;
        private Match _match;
        private Regex _regex;

        public int Count { get { return _match.Groups.Count; } }
        public string [] Keys { get; private set; }
        public int [] Indices { get; private set; }

        public string Value
        {
            get
            {
                if (_defaultGroupValueKey != null)
                {
                    return ValueOf(_defaultGroupValueKey);
                }

                return ValueOf(_defaultGroupValueIndex);
            }
        }
        public Regex Regex
        {
            get
            {
                return _regex;
            }
            set
            {
                _regex = value;
                Keys = Regex.GetGroupNames();
                Indices = Regex.GetGroupNumbers();
            }
        }

        public RegexFunctionElement(string pattern, RegexOptions options = RegexOptions.None, int defaultGroupValueIndex = 1)
            : this(new Regex(pattern, options), defaultGroupValueIndex)
        {
        }

        public RegexFunctionElement(string pattern, RegexOptions options, string defaultGroupValueKey)
            : this(new Regex(pattern, options), defaultGroupValueKey)
        {
        }

        public RegexFunctionElement(Regex regexWithGroup, int defaultGroupValueIndex = 0)
        {
            Regex = regexWithGroup;
            _defaultGroupValueIndex = defaultGroupValueIndex;
            _defaultGroupValueKey = null;

            if (!Indices.Contains(defaultGroupValueIndex))
                ThrowException(defaultGroupValueIndex);
        }

        public RegexFunctionElement(Regex regexWithGroup, string defaultGroupValueKey)
        {
            Regex = regexWithGroup;
            _defaultGroupValueKey = defaultGroupValueKey;
            _defaultGroupValueIndex = 0;

            if (defaultGroupValueKey == null ||
                !Keys.Contains(defaultGroupValueKey))
                ThrowException(defaultGroupValueKey ?? "undefined");
        }

        public void Match(string content)
        {
            _match = Regex.Match(content);
        }

        string IRegexFunctionElement.this[int index]
        {
            get { return this[index]; }
        }

        string IRegexFunctionElement.this[string key]
        {
            get { return this[key]; }
        }

        public string this[int index]
        {
            get { return ValueOf(index); }
        }

        public string this[string key]
        {
            get { return ValueOf(key); }
        }

        private string ValueOf(int index)
        {
            if (!Indices.Contains(index))
                ThrowException(index);

            return _match.Groups[index].Value;
        }

        private string ValueOf(string key)
        {
            if (!Keys.Contains(key))
                ThrowException(key);

            return _match.Groups[key].Value;
        }

        private static void ThrowException(object key)
        {
            throw new KeyNotFoundException("Couldn't find element " + key + " in collection.");
        }
    }
}