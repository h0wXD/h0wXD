
namespace h0wXD.Email.Interfaces
{
    public interface ITemplateDao
    {
        bool PathExists(string _sPath);
        string [] GetFileList(string _sPath, string _sSearchPattern);
        string ReadFile(string _sFileName);
    }
}
