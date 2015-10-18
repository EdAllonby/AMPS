using System.Configuration;

namespace Shared.Configuration
{
    /// <summary>
    /// A facade for the static ConfigurationManager
    /// </summary>
    public sealed class AppConfiguration : IConfiguration
    {
        public string ConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        public System.Configuration.Configuration OpenConfiguration(ConfigurationUserLevel userLevel)
        {
            return ConfigurationManager.OpenExeConfiguration(userLevel);
        }

        public void RefreshSection(string sectionName)
        {
            ConfigurationManager.RefreshSection(sectionName);
        }
    }
}
