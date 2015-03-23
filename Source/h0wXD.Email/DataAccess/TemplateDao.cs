using System.IO;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.DataAccess
{
    public class TemplateDao : ITemplateDao
    {
        public bool PathExists(string _sPath)
        {
            return Directory.Exists(_sPath);
        }

        public string [] GetFileList(string _sPath, string _sSearchPattern)
        {
            if (!Directory.Exists(_sPath))
            {
                return new string [] {};
            }

            return Directory.GetFiles(_sPath, _sSearchPattern);
        }

        public string ReadFile(string _sFileName)
        {
            return File.ReadAllText(_sFileName);
        }
    }
}
