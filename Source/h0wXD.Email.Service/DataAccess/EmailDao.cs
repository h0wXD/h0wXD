using System.IO;
using System.Linq;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Interfaces;

namespace h0wXD.Email.Service.DataAccess
{
    public class EmailDao : IEmailDao
    {
        private readonly string m_sPathToUnprocessed;
        private readonly string m_sPathToArchive;

        public EmailDao(IEncryptedConfiguration _config)
        {
            var sDropFolderPath = _config.Read<string>(TechnicalConstants.Settings.DropFolder);
            
            m_sPathToUnprocessed = Path.Combine(sDropFolderPath, TechnicalConstants.UnprocessedFolder);
            m_sPathToArchive = Path.Combine(sDropFolderPath, TechnicalConstants.ArchiveFolder);

            foreach (var sPath in new [] {m_sPathToUnprocessed, m_sPathToArchive}.Where(sPath => !Directory.Exists(sPath)))
            {
                Directory.CreateDirectory(sPath);
            }
        }

        public string [] FindEmailsByFileMask(string _sPath, string _sFileMask)
        {
            return Directory.GetFiles(_sPath, _sFileMask);
        }

        public bool IsProcessed(string _sEmailFile)
        {
            return !File.Exists(_sEmailFile);
        }

        public void MoveToUnprocessed(string _sEmailFile)
        {
            MoveFile(_sEmailFile, m_sPathToUnprocessed);
        }

        public void MoveToArchive(string _sEmailFile)
        {
            MoveFile(_sEmailFile, m_sPathToArchive);
        }

        private void MoveFile(string _sFile, string _sDestinationPath)
        {
            var iNumber = 1;
            var sNewFile = Path.Combine(_sDestinationPath, Path.GetFileName(_sFile));
            var sExtension = Path.GetExtension(_sFile);

            while (File.Exists(sNewFile))
            {
                sNewFile = Path.Combine(_sDestinationPath, Path.GetFileNameWithoutExtension(_sFile) + "_" + iNumber.ToString() + sExtension);
                iNumber++;
            }

            File.Move(_sFile, sNewFile);
        }

        public void Delete(string _sEmailFile)
        {
            File.Delete(_sEmailFile);
        }
    }
}
