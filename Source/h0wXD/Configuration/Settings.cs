using System;
using h0wXD.Configuration.Interfaces;
using System.IO;
using SysConf = System.Configuration;

namespace h0wXD.Configuration
{
    public class Settings : ISettings
    {
        protected SysConf.Configuration Configuration;
        protected SysConf.ClientSettingsSection DefaultClientSection;
        protected bool Opened;
        protected string DefaultClientSectionName;

        public string DefaultSection
        {
            get
            {
                return DefaultClientSectionName;
            }
            set
            {
                DefaultClientSectionName = value;
                DefaultClientSection = (SysConf.ClientSettingsSection) Configuration.GetSection(DefaultClientSectionName);
            }
        }

        public Settings()
        {
            Opened = false;
        }

        public virtual bool Open(string sectionName = null, string fileName = null, SysConf.ConfigurationUserLevel userLevel = SysConf.ConfigurationUserLevel.None)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Configuration = SysConf.ConfigurationManager.OpenExeConfiguration(userLevel);
            }
            else
            {
                var configurationFileMap = new SysConf.ExeConfigurationFileMap();
                configurationFileMap.ExeConfigFilename = fileName;
                Configuration = SysConf.ConfigurationManager.OpenMappedExeConfiguration(configurationFileMap, userLevel);
            }

            DefaultSection = sectionName ?? "userSettings/" + Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName.Trim()).Replace(".vshost", "") + ".Settings";

            if (DefaultClientSection == null)
            {
                throw new FileLoadException("Could find setting " + DefaultSection + " in file.");
            }

            return Opened = Configuration != null;
        }

        protected void RequireOpenedConfiguration()
        {
            if (!Opened)
                throw new AccessViolationException("No file is open!");
        }

        protected virtual T Read<T>(SysConf.ClientSettingsSection section, string key, T value = default(T))
        {
            var settingElement = section.Settings.Get(key);

            if (settingElement != null)
            {
                try
                {
                    var innerValue = ((SysConf.SettingValueElement)(settingElement.ElementInformation.Properties["value"].Value)).ValueXml.InnerText;

                    return (T)Convert.ChangeType(innerValue, typeof(T));
                }
                catch (Exception)
                {
                    if (value.Equals(default(T)))
                    {
                        throw;
                    }
                }
            }

            return value;
        }

        protected virtual void Write<T>(SysConf.ClientSettingsSection section, string key, T value = default(T))
        {
            var settingElement = section.Settings.Get(key);

            if (settingElement != null)
            {
                ((SysConf.SettingValueElement)(settingElement.ElementInformation.Properties["value"].Value)).ValueXml.InnerText = value.ToString();
            }
        }

        public virtual T Read<T>(string fullSectionName, string key, T value = default(T))
        {
            RequireOpenedConfiguration();

            var section = (SysConf.ClientSettingsSection)Configuration.GetSection(fullSectionName);
            return Read(section, key, value);
        }

        public virtual T Read<T>(string key, T value = default(T))
        {
            RequireOpenedConfiguration();

            return Read(DefaultClientSection, key, value);
        }

        public virtual void Write(string fullSectionName, string key, object value)
        {
            RequireOpenedConfiguration();

            var section = (SysConf.ClientSettingsSection)Configuration.GetSection(fullSectionName);
            Write(section, key, value);
        }

        public virtual void Write(string key, object value)
        {
            RequireOpenedConfiguration();

            Write(DefaultClientSection, key, value);
        }

        public virtual void Save()
        {
            RequireOpenedConfiguration();

            Configuration.Save();
        }
    }
}
