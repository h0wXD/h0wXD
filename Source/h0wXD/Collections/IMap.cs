
namespace h0wXD.Collections
{
    public interface IMap<in TKey, TValue> where TValue : class
    {
        TValue this[TKey _key] { get; set; }
    }
}
