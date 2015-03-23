
namespace h0wXD.Configuration.Interfaces
{
    public interface IConfiguration
    {
        T Read<T>(string _sConfigKey, T _defaultValue = default(T));

        void Write(string _sConfigKey, object _value);

        void Save();
    }
}
