using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h0wXD.Email.Service.Injection;
using Ninject;
using Ninject.Modules;

namespace h0wXD.Email.Service.TestApp.Injection
{
    public class ProductionKernel : StandardKernel
    {
        private static ProductionKernel ms_productionKernel;

        public static IKernel Instance { get { return ms_productionKernel ?? (ms_productionKernel = new ProductionKernel()); } }

        public ProductionKernel()
        {
            Load(new INinjectModule [] 
            {
                new ProductionModule()
            });
        }
    }
}
