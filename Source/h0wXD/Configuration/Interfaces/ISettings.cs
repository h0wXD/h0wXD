
namespace h0wXD.Configuration.Interfaces
{
    public interface ISettings
    {
        string DefaultSection { get; set; }

        bool Open(string sectionName = null, string fileName = null, System.Configuration.ConfigurationUserLevel userLevel = System.Configuration.ConfigurationUserLevel.None);

        T Read<T>(string fullSectionName, string key, T value = default(T));
        T Read<T>(string key, T value = default(T));

        void Write(string fullSectionName, string key, object value);
        void Write(string key, object value);

        void Save();
    }
}
