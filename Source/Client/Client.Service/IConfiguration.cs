using System.Configuration;

namespace Client.Service
{
    public interface IConfiguration
    {
        string ConnectionString(string key);

        Configuration OpenConfiguration(ConfigurationUserLevel userLevel);

        void RefreshSection(string sectionName);
    }
}