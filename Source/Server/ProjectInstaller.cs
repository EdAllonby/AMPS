using System.ComponentModel;
using System.Configuration.Install;

namespace Server
{
    /// <summary>
    /// An installer for the server.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// Run the installer for the server.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}