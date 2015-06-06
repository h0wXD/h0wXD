
namespace h0wXD.Email.Service.Interfaces
{
    public interface IEmailManager
    {
        void ProcessEmail(string fileName);
        void ProcessExistingEmails();
    }
}
