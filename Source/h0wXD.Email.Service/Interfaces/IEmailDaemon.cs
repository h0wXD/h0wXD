
namespace h0wXD.Email.Service.Interfaces
{
    public interface IEmailDaemon
    {
        void Pause();
        void Continue();
        void Execute();
    }
}
