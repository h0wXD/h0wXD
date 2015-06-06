using System;
using h0wXD.Helpers;

namespace h0wXD.Service
{
    public class ServiceConsoleManager : ServiceControlManager
    {
        public ServiceConsoleManager(string servicePath, string serviceName) :
            base(servicePath, serviceName)
        {
        }

        public bool HandleCommandlineParameter(string parameter)
        {
            if (parameter.StartsWith(TechnicalConstants.Service.InstallParameter, 2))
            {
                Install();
                Console.WriteLine("Service {0} has been installed.\nPath: {1}", ServiceName, ServicePath);
                return true;
            }
            
            if (parameter.StartsWith(TechnicalConstants.Service.UninstallParameter, 2))
            {
                Uninstall();
                Console.WriteLine("Service {0} has been uninstalled.\nPath: {1}", ServiceName, ServicePath);
                return true;
            }

            if (parameter.StartsWith(TechnicalConstants.Service.StopParameter, 4))
            {
                Stop();
                Console.WriteLine("Service {0} has been stopped.", ServiceName);
                return true;
            }
            
            if (parameter.StartsWith(TechnicalConstants.Service.StartParameter, 2))
            {
                Start();
                Console.WriteLine("Service {0} has been started.", ServiceName);
                return true;
            }

            return false;
        }
        
        public void DisplayUsage()
        {
            Console.Write("Usage:\n{0} {1}\tInstalls this service.\n{0} {2}\tUninstalls this service.\n{0} {3}\t\tStarts this service.\n{0} {4}\t\tStops this service.\n",
                AppDomain.CurrentDomain.FriendlyName,
                TechnicalConstants.Service.InstallParameter,
                TechnicalConstants.Service.UninstallParameter,
                TechnicalConstants.Service.StartParameter,
                TechnicalConstants.Service.StopParameter);
        }
    }
}
