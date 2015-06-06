using h0wXD.Configuration.Interfaces;
using h0wXD.Email.Interfaces;

namespace h0wXD.Email.Batch.Configuration
{
    public class DropEmailConfiguration : IDropEmailConfiguration
    {
        public string DropFolder { get; private set; }

        public DropEmailConfiguration(ISettings settings)
        {
            DropFolder = settings.Read<string>(TechnicalConstants.Settings.DropFolder);
        }
    }
}
