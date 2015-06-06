using Ninject;
using Ninject.Modules;

namespace h0wXD.Email.Batch.Injection
{
    public class ProductionKernel : StandardKernel
    {
        private static ProductionKernel Kernel;

        public static IKernel Instance { get { return Kernel ?? (Kernel = new ProductionKernel()); } }

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
