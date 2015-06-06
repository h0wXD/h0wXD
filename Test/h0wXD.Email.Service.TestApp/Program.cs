using System;
using System.Windows.Forms;
using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Service.Injection;
using Ninject;

namespace h0wXD.Email.Service.TestApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ProductionKernel.Instance.Get<ISettings>().Open();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ProductionKernel.Instance.Get<EmailServiceForm>());
        }
    }
}
