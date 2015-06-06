using System.IO;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.DataAccess
{
    public class TemplateDao : ITemplateDao
    {
        public bool PathExists(string path)
        {
            return Directory.Exists(path);
        }

        public string [] GetFileList(string directory, string fileMask)
        {
            if (!Directory.Exists(directory))
            {
                return new string [] {};
            }

            return Directory.GetFiles(directory, fileMask);
        }

        public string ReadFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
