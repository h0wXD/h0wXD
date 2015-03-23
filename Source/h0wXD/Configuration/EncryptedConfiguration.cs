using SysConf = System.Configuration;

namespace h0wXD.Configuration
{
    /// <summary>
    /// EncryptedConfiguration allows reading and writing of system level encryption.
    /// Existing configuration automatically encrypts the configuration on first run if it is unencrypted.
    /// Settings.settings files should be deleted from new Form applications (and others)
	/// Just configure App.config and deploy it with the compiled executable name.config
    /// </summary>
    public class EncryptedConfiguration : Configuration
    {
        private readonly string m_sConfigurationEncryptionProvider;

        /// <summary>
        /// Constructs an instance of EncryptedConfiguration using the provided configuration encryption provider.
        /// </summary>
        /// <param name="_sConfigurationEncryptionProvider">RsaProtectedConfigurationProvider or DataProtectionConfigurationProvider</param>
        public EncryptedConfiguration(string _sConfigurationEncryptionProvider = "RsaProtectedConfigurationProvider")
        {
            m_sConfigurationEncryptionProvider = _sConfigurationEncryptionProvider;

            if (!m_section.SectionInformation.IsProtected)
            {
                Save();
            }
        }

        /// <summary>
        /// Reads an encrypted configuration value and automatically casts it to the provided type.
        /// </summary>
        /// <typeparam name="T">Type to cast the configuration key to.</typeparam>
        /// <param name="_sConfigurationKey">Key to read.</param>
        /// <param name="_defaultValue">Default value to return.</param>
        /// <returns>Value of the configuration key.</returns>
        public T Read<T>(string _sConfigurationKey, T _defaultValue = default(T))
        {
            if (m_section.SectionInformation.IsProtected)
            {
                m_section.SectionInformation.UnprotectSection();
            }
            
            return base.Read(_sConfigurationKey, _defaultValue);
        }

        /// <summary>
        /// Writes a value in the encrypted configuration. Notice: the Save method still has to be called.
        /// </summary>
        /// <param name="_sConfigurationKey">The configuration key to write to.</param>
        /// <param name="_value">The value to write</param>
        public void Write(string _sConfigurationKey, object _value)
        {
            if (m_section.SectionInformation.IsProtected)
            {
                m_section.SectionInformation.UnprotectSection();
            }
            
            base.Write(_sConfigurationKey, _value);
        }

        /// <summary>
        /// Saves the edited configuration.
        /// </summary>
        public void Save()
        {
            if (!m_section.SectionInformation.IsProtected &&
                !m_section.SectionInformation.IsLocked)
            {
                m_section.SectionInformation.ProtectSection(m_sConfigurationEncryptionProvider);
            }

            base.Save();
        }
    }
}
