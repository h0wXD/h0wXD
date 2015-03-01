
namespace h0wXD.Email.Service.Interfaces
{
    public interface IEmailDao
    {
        string [] FindEmailsByFileMask(string _sPath, string _sFileMask);
        bool IsProcessed(string _sEmailFile);
        void MoveToError(string _sEmailFile);
        void MoveToArchive(string _sEmailFile);
        void Delete(string _sEmailFile);
    }
}
