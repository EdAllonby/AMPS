using System;
using System.Configuration;
using log4net;

namespace Shared.Configuration
{
    public class AppConfigManager : IService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (AppConfigManager));

        private readonly IConfiguration configuration;

        public AppConfigManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string FindStoredValue(string key)
        {
            try
            {
                string value = configuration.ConnectionString(key);
                Log.DebugFormat("Found stored configuration value [{0}] for key [{1}]", value, key);
                return configuration.ConnectionString(key);
            }
            catch (NullReferenceException)
            {
                // This isn't too much of an issue. If we can't find a default value in the config, return an empty string.
                // The worst that can happen is that the user will have to manually enter a connection setting.
                Log.WarnFormat("Could not find a value for key: {0}", key);
                return string.Empty;
            }
        }

        public void UpdateSetting(string key, string value)
        {
            try
            {
                System.Configuration.Configuration config = configuration.OpenConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
                config.Save(ConfigurationSaveMode.Modified, true);

                Log.DebugFormat("Updated stored configuration value [{0}] for key [{1}]", value, key);


                configuration.RefreshSection("connectionStrings");
            }
            catch (Exception)
            {
                Log.ErrorFormat("Failed to change configuration setting for key {0}", key);
            }
        }
    }
}