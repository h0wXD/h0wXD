using System.Collections.Generic;
using h0wXD.Collections.Interfaces;

namespace h0wXD.Collections
{
    public class Map<TKey, TValue> : Dictionary<TKey, TValue>, IMap<TKey, TValue> //where TValue : class
    {
        TValue IMap<TKey, TValue>.this[TKey key]
        {
            get
            {
                return this[key];
            }
            set
            {
                this[key] = value;
            }
        }

        public new TValue this[TKey key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                Add(key, value);
            }
        }
    }
}
