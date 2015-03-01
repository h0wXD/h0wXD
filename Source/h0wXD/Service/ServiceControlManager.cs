using System.Configuration.Install;
using System.ServiceProcess;

namespace h0wXD.Service
{
    public class ServiceControlManager
    {
        private readonly ServiceController m_serviceController;
        protected readonly string m_sServiceLocation;
        protected readonly string m_sServiceName;

        public ServiceControlManager(string _sServiceLocation, string _sServiceName)
        {
            m_sServiceLocation = _sServiceLocation;
            m_sServiceName = _sServiceName;
            m_serviceController = new ServiceController(_sServiceName);
        }

        public void Install()
        {
            ManagedInstallerClass.InstallHelper(new [] { "/LogToConsole=false", m_sServiceLocation });
        }

        public void Uninstall()
        {
            ManagedInstallerClass.InstallHelper(new [] { "/u", "/LogToConsole=false", m_sServiceLocation });
        }
        
        public void Start()
        {
            if (m_serviceController.Status == ServiceControllerStatus.Stopped)
            {
                m_serviceController.Start();
            }
        }

        public void Stop()
        {
            if (m_serviceController.Status == ServiceControllerStatus.Running)
            {
                m_serviceController.Stop();
            }
        }
    }
}
