using h0wXD.Configuration;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Batch.Configuration;
using h0wXD.Email.Batch.Managers;
using h0wXD.Email.Behaviors;
using h0wXD.Email.Interfaces;
using h0wXD.Logging.Interfaces;
using Ninject.Modules;

namespace h0wXD.Email.Batch.Injection
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<LogManager>().InSingletonScope();
            Bind<ISettings>().To<EncryptedSettings>().InSingletonScope();
            Bind<IDropEmailConfiguration>().To<DropEmailConfiguration>().InSingletonScope();
            Bind<ISendMailBehavior>().To<DropEmailBehavior>().InSingletonScope();
        }
    }
}
