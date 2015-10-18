using System.Configuration;

namespace Shared.Configuration
{
    public interface IConfiguration
    {
        string ConnectionString(string key);

        System.Configuration.Configuration OpenConfiguration(ConfigurationUserLevel userLevel);

        void RefreshSection(string sectionName);
    }
}