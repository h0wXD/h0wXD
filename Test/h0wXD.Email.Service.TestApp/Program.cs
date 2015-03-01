using System;
using System.Windows.Forms;
using h0wXD.Email.Service.TestApp.Injection;
using Ninject;

namespace h0wXD.Email.Service.TestApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ProductionKernel.Instance.Get<EmailServiceForm>());
        }
    }
}
