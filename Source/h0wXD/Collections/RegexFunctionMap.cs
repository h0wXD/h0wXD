using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace h0wXD.Collections
{
    public class RegexFunctionMap : IEnumerable<KeyValuePair<int, RegexFunctionMap.Entry>>, IEnumerable
    {
        public class Entry
        {
            private Match m_match;
            private readonly Func<Match, string> m_valueFunction;

            public Regex Regex { get; private set; }

            public string Value
            {
                get
                {
                    if (m_match.Groups.Count == 0)
                    {
                        throw new InvalidOperationException("Content did not match the Regex pattern. No Regex groups have been found.");
                    }
                    return m_valueFunction(m_match);
                }
            }

            public Entry(Regex _regex, Func<Match, string> _value)
            {
                Regex = _regex;
                m_valueFunction = _value;
            }

            internal void Match(string _sContent)
            {
                m_match = Regex.Match(_sContent);
            }
        }

        private readonly Dictionary<int, Entry> m_map;

        public RegexFunctionMap()
        {
            m_map = new Dictionary<int, Entry>();
        }

        public void Add(int _iKey, Entry _entry)
        {
            m_map[_iKey] = _entry;
        }

        public void Match(string _sContent)
        {
            foreach (var keypair in m_map)
            {
                keypair.Value.Match(_sContent);
            }
        }

        public void Clear()
        {
            m_map.Clear();
        }

        public string this[int _iKey]
        {
            get
            {
                if (!m_map.ContainsKey(_iKey))
                {
                    throw new KeyNotFoundException("Couldn't find key " + _iKey + " in collection.");
                }
                return m_map[_iKey].Value;
            }
        }

        public IEnumerator<KeyValuePair<int, Entry>> GetEnumerator()
        {
            return m_map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
