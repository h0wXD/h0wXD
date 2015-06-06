using System.Collections.Generic;
using SysConf = System.Configuration;

namespace h0wXD.Configuration
{
    public class EncryptedSettings : Settings
    {
        private readonly HashSet<SysConf.ClientSettingsSection> _accessedSections;

        public string ConfigurationEncryptionProvider { get; set; }

        public EncryptedSettings(string configurationEncryptionProvider = "RsaProtectedConfigurationProvider")
        {
            ConfigurationEncryptionProvider = configurationEncryptionProvider;
            _accessedSections = new HashSet<SysConf.ClientSettingsSection>();
        }

        public override bool Open(string sectionName = null, string fileName = null, SysConf.ConfigurationUserLevel userLevel = SysConf.ConfigurationUserLevel.None)
        {
            base.Open(sectionName, fileName, userLevel);

            if (Opened)
            {
                _accessedSections.Add(DefaultClientSection);

                if (!DefaultClientSection.SectionInformation.IsProtected)
                {
                    Save();
                }
            }

            return Opened;
        }
        
        protected override T Read<T>(SysConf.ClientSettingsSection section, string key, T value = default(T))
        {
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
            }

            _accessedSections.Add(section);

            return base.Read(section, key, value);
        }

        protected override void Write<T>(SysConf.ClientSettingsSection section, string key, T value = default(T))
        {
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
            }

            _accessedSections.Add(section);

            base.Write(section, key, value);
        }

        public override void Save()
        {
            RequireOpenedConfiguration();

            if (!string.IsNullOrWhiteSpace(ConfigurationEncryptionProvider))
            {
                foreach (var section in _accessedSections)
                {
                    if (!section.SectionInformation.IsProtected && 
                        !section.SectionInformation.IsLocked)
                    {
                        section.SectionInformation.ProtectSection(ConfigurationEncryptionProvider);
                    }
                }
            }

            base.Save();
        }
    }
}
