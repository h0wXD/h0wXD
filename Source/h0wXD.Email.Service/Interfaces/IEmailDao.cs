using System.Net.Mail;

namespace h0wXD.Email.Service.Interfaces
{
    public interface IEmailDao
    {
        string [] FindEmailsByFileMask(string directory, string fileMask);
        bool IsProcessed(string emailFilePath);
        void MoveToError(string emailFilePath);
        void MoveToArchive(string emailFilePath);
        void Delete(string emailFilePath);
        bool Send(MailMessage mailMessage);
        string Load(string fileName);
    }
}
