
namespace h0wXD.Email.Interfaces
{
    public interface ITemplateDao
    {
        bool PathExists(string path);
        string [] GetFileList(string directory, string fileMask);
        string ReadFile(string fileName);
    }
}
