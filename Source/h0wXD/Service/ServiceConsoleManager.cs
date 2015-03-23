using System;
using System.Reflection;

namespace h0wXD.Service
{
    public class ServiceConsoleManager : ServiceControlManager
    {
        public ServiceConsoleManager(string _sServiceLocation, string _sServiceName) :
            base(_sServiceLocation, _sServiceName)
        {
        }

        public bool HandleCommandlineParameter(string _sParameter)
        {
            var sMessage = String.Empty;

            if (_sParameter.StartsWith(TechnicalConstants.Service.InstallParameter, 2))
            {
                Install();
                sMessage = String.Format("Service {0} has been installed.|Location: {1}", m_sServiceName, m_sServiceLocation);
            }
            else if (_sParameter.StartsWith(TechnicalConstants.Service.UninstallParameter, 2))
            {
                Uninstall();
                sMessage = String.Format("Service {0} has been uninstalled.|Location: {1}", m_sServiceName, m_sServiceLocation);
            }
            else if (_sParameter.StartsWith(TechnicalConstants.Service.StopParameter, 4))
            {
                Stop();
                sMessage = String.Format("Service {0} has been stopped.", m_sServiceName);
            }
            else if (_sParameter.StartsWith(TechnicalConstants.Service.StartParameter, 2))
            {
                Start();
                sMessage = String.Format("Service {0} has been started.", m_sServiceName);
            }

            if (sMessage != String.Empty)
            {
                Console.WriteLine(sMessage.Replace("|", System.Environment.NewLine));
                return true;
            }

            return false;
        }
        
        public void DisplayUsage()
        {
            var sExeName = AppDomain.CurrentDomain.FriendlyName;
            var sMessage = String.Format("Usage:|{0} {1}\tInstalls this service.|{0} {2}\tUninstalls this service.|{0} {3}\t\tStarts this service.|{0} {4}\t\tStops this service.", 
                sExeName, 
                TechnicalConstants.Service.InstallParameter,
                TechnicalConstants.Service.UninstallParameter,
                TechnicalConstants.Service.StartParameter,
                TechnicalConstants.Service.StopParameter);
            Console.WriteLine(sMessage.Replace("|", System.Environment.NewLine));
        }
    }
}
