using System;
using h0wXD.Configuration.Interfaces;
using System.IO;
using SysConf = System.Configuration;

namespace h0wXD.Configuration
{
    /// <summary>
    /// Settings.settings files should be deleted from new Form applications (and others)
	/// Just configure App.config and deploy it with the compiled executable name.config
    /// </summary>
    public class Configuration : IConfiguration
    {
        protected readonly SysConf.Configuration m_configuration;
        protected readonly SysConf.ClientSettingsSection m_section;

        /// <summary>
        /// Constructs an instance of Configuration.
        /// </summary>
        public Configuration()
        {
            m_configuration = SysConf.ConfigurationManager.OpenExeConfiguration(SysConf.ConfigurationUserLevel.None);
            var sConfigSectionName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName).Replace(".vshost", "");

            m_section = (SysConf.ClientSettingsSection)m_configuration.GetSection("userSettings/" + sConfigSectionName + ".Settings");
        }

        /// <summary>
        /// Reads a configuration value and automatically casts it to the provided type.
        /// </summary>
        /// <typeparam name="T">Type to cast the configuration key to.</typeparam>
        /// <param name="_sConfigurationKey">Key to read.</param>
        /// <param name="_defaultValue">Default value to return.</param>
        /// <returns>Value of the configuration key.</returns>
        public T Read<T>(string _sConfigurationKey, T _defaultValue = default(T))
        {
            var settingElement = m_section.Settings.Get(_sConfigurationKey);

            if (settingElement != null)
            {
                try
                {
                    var value = ((SysConf.SettingValueElement) (settingElement.ElementInformation.Properties["value"].Value)).ValueXml.InnerText;

                    return (T) Convert.ChangeType(value, typeof (T));
                }
                catch (Exception)
                {
                    if (_defaultValue.Equals(default(T)))
                    {
                        throw;
                    }
                }
            }

            return _defaultValue;
        }

        /// <summary>
        /// Writes a value in the configuration. Notice: the Save method still has to be called.
        /// </summary>
        /// <param name="_sConfigurationKey">The configuration key to write to.</param>
        /// <param name="_value">The value to write</param>
        public void Write(string _sConfigurationKey, object _value)
        {
            var settingElement = m_section.Settings.Get(_sConfigurationKey);

            if (settingElement != null)
            {
                ((SysConf.SettingValueElement)(settingElement.ElementInformation.Properties["value"].Value)).ValueXml.InnerText = _value.ToString();
            }
        }

        /// <summary>
        /// Saves the edited configuration.
        /// </summary>
        public void Save()
        {
            m_configuration.Save();
        }
    }
}
