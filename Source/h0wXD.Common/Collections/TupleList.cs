using System;
using System.Collections.Generic;

namespace h0wXD.Common.Collections
{
    /// <summary>
    /// Allows initializing a list of tuples easily using brackets {}
    /// </summary>
    /// <typeparam name="T1">Type of item1</typeparam>
    /// <typeparam name="T2">Type of item2</typeparam>
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 _item1, T2 _item2)
        {
            Add(new Tuple<T1, T2>(_item1, _item2));
        }
    }
    
    /// <summary>
    /// Allows initializing a list of tuples easily using brackets {}
    /// </summary>
    /// <typeparam name="T1">Type of item1</typeparam>
    /// <typeparam name="T2">Type of item2</typeparam>
    /// <typeparam name="T3">Type of item3</typeparam>
    public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public void Add(T1 _item1, T2 _item2, T3 _item3)
        {
            Add(new Tuple<T1, T2, T3>(_item1, _item2, _item3));
        }
    }
}