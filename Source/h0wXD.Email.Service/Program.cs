using System;
using System.Reflection;
using System.ServiceProcess;
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
                var sServiceLocation = Assembly.GetExecutingAssembly().Location;
                var sServiceName = (new ProjectInstaller()).ServiceName;
                var serviceConsoleHandler = new ServiceConsoleManager(sServiceLocation, sServiceName);

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
                ServiceBase.Run(new ServiceBase [] 
                { 
                    ProductionKernel.Instance.Get<EmailService>()
                });
            }
        }
    }
}
