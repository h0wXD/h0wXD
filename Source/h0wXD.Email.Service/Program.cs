using System;
using System.Reflection;
using System.ServiceProcess;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Injection;
using h0wXD.Email.Service.Installer;
using h0wXD.Service;
using Ninject;

namespace h0wXD.Email.Service
{
    static class Program
    {
        static void Main(string [] _sArgs)
        {
            //if (Environment.UserInteractive)
            //{
                var servicePath = Assembly.GetExecutingAssembly().Location;
                var serviceName = (new ProjectInstaller()).ServiceName;
                var serviceConsoleHandler = new ServiceConsoleManager(servicePath, serviceName);

                if (_sArgs.Length > 0)
                {
                    if (serviceConsoleHandler.HandleCommandlineParameter(_sArgs[0]))
                    {
                        return;
                    }
                    serviceConsoleHandler.DisplayUsage();
                }
            //}
            else
            {
                ProductionKernel.Instance.Get<ISettings>().Open();

                ServiceBase.Run(new ServiceBase [] 
                { 
                    ProductionKernel.Instance.Get<EmailService>()
                });
            }
        }
    }
}
