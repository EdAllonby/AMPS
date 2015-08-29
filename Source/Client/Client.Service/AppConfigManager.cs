using System;
using System.Configuration;
using log4net;

namespace Client.Service
{
    public static class AppConfigManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (AppConfigManager));

        public static string FindStoredValue(string key)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (NullReferenceException)
            {
                // This isn't too much of an issue. If we can't find a default value in the config, return an empty string.
                // The worst that can happen is that the user will have to manually enter a connection setting.
                Log.WarnFormat("Could not find a value for key: {0}", key);
                return string.Empty;
            }
        }

        public static void UpdateSetting(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
                config.Save(ConfigurationSaveMode.Modified, true);

                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception)
            {
                Log.ErrorFormat("Failed to change configuration setting for key {0}", key);
            }
        }
    }
}