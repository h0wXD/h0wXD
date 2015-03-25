using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h0wXD.Collections
{
    public class Map<TKey, TValue> : Dictionary<TKey, TValue>, IMap<TKey, TValue> where TValue : class
    {
        TValue IMap<TKey, TValue>.this[TKey _key]
        {
            get
            {
                TValue value;
                TryGetValue(_key, out value);
                return value;
            }
            set
            {
                Add(_key, value);
            }
        }
    }
}
