using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h0wXD.Collections
{
    public interface IMap<in TKey, TValue> where TValue : class
    {
        TValue this[TKey _key] { get; set; }
    }
}
