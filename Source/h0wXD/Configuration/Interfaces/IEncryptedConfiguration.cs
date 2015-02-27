
namespace h0wXD.Configuration.Interfaces
{
    public interface IEncryptedConfiguration
    {
        T Read<T>(string _sConfigKey);

        void Write(string _sConfigKey, object _value);

        void Save();
    }
}
