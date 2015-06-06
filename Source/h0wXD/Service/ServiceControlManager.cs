using System.Configuration.Install;
using System.ServiceProcess;

namespace h0wXD.Service
{
    public class ServiceControlManager
    {
        private readonly ServiceController _serviceController;
        protected readonly string ServicePath;
        protected readonly string ServiceName;

        public ServiceControlManager(string servicePath, string serviceName)
        {
            ServicePath = servicePath;
            ServiceName = serviceName;
            _serviceController = new ServiceController(serviceName);
        }

        public void Install()
        {
            ManagedInstallerClass.InstallHelper(new [] { "/LogToConsole=false", ServicePath });
        }

        public void Uninstall()
        {
            ManagedInstallerClass.InstallHelper(new [] { "/u", "/LogToConsole=false", ServicePath });
        }
        
        public void Start()
        {
            if (_serviceController.Status == ServiceControllerStatus.Stopped)
            {
                _serviceController.Start();
            }
        }

        public void Stop()
        {
            if (_serviceController.Status == ServiceControllerStatus.Running)
            {
                _serviceController.Stop();
            }
        }
    }
}
