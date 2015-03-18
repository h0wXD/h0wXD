using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace h0wXD.Collections
{
    public class ReverseMap : IEnumerable<KeyValuePair<int, ReverseMap.Entry>>, IEnumerable
    {
        public class Entry
        {
            private Match m_match;
            private Func<Match, string> m_valueFunc;

            public Regex Regex { get; private set; }
            public string Value { get { return m_valueFunc(m_match); } }

            public Entry(Regex _regex, Func<Match, string> _value)
            {
                Regex = _regex;
                m_valueFunc = _value;
            }

            public void Match(string _sContent)
            {
                m_match = Regex.Match(_sContent);
            }
        }

        private readonly Dictionary<int, Entry> m_map;

        public ReverseMap()
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

        public string this[int _iKey]
        {
            get
            {
                if (!m_map.ContainsKey(_iKey))
                {
                    return null;
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
