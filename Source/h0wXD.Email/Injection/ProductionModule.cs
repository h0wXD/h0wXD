using h0wXD.Email.Builders;
using h0wXD.Email.DataAccess;
using h0wXD.Email.Interfaces;
using h0wXD.Email.Managers;
using h0wXD.Email.Parsers;
using Ninject.Modules;

namespace h0wXD.Email.Injection
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITemplateManager>().To<TemplateManager>().InSingletonScope();
            Bind<ITemplateDao>().To<TemplateDao>().InSingletonScope();
            Bind<IEmailDao>().To<EmailDao>().InSingletonScope();
            Bind<IEmailMessageBuilder>().To<EmailMessageBuilder>().InSingletonScope();
            Bind<IEmailMessageParser>().To<EmailMessageParser>().InSingletonScope();
        }
    }
}
