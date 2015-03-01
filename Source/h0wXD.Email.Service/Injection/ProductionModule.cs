using h0wXD.Configuration;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Daemon;
using h0wXD.Email.Service.DataAccess;
using h0wXD.Email.Service.Interfaces;
using h0wXD.Email.Service.Managers;
using h0wXD.IO;
using h0wXD.IO.Interfaces;
using h0wXD.Logging.Interfaces;
using Ninject.Modules;

namespace h0wXD.Email.Service.Injection
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<LogManager>().InSingletonScope();
            Bind<IEncryptedConfiguration>().To<EncryptedConfiguration>().InSingletonScope();
            Bind<IDirectoryWatcher>().To<DirectoryWatcher>().InSingletonScope();
            Bind<IEmailDaemon>().To<EmailDaemon>().InSingletonScope();
            Bind<IEmailManager>().To<EmailManager>().InSingletonScope();
            Bind<IEmailDao>().To<EmailDao>().InSingletonScope();
        }
    }
}
