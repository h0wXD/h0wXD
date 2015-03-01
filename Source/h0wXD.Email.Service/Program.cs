using System.ServiceProcess;
using h0wXD.Email.Service.Injection;
using Ninject;

namespace h0wXD.Email.Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new ServiceBase [] 
            { 
                ProductionKernel.Instance.Get<EmailService>()
            });
        }
    }
}
