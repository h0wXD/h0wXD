using Ninject;
using Ninject.Modules;

namespace h0wXD.Email.Batch.Injection
{
    public class ProductionKernel : StandardKernel
    {
        private static ProductionKernel ms_productionKernel;

        public static IKernel Instance { get { return ms_productionKernel ?? (ms_productionKernel = new ProductionKernel()); } }

        public ProductionKernel()
        {
            Load(new INinjectModule [] 
            {
                new ProductionModule(),
                new Email.Injection.ProductionModule()
            });
        }
    }
}
