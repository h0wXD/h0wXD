
namespace h0wXD.Collections.Interfaces
{
    public interface IMap<in TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}
