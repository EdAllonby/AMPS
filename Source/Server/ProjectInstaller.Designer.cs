namespace Server
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fypServerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.fypServerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // fypServerProcessInstaller
            // 
            this.fypServerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.fypServerProcessInstaller.Password = null;
            this.fypServerProcessInstaller.Username = null;
            // 
            // fypServerInstaller
            // 
            this.fypServerInstaller.Description = "The FYP Server Service";
            this.fypServerInstaller.DisplayName = "FYP Server Service";
            this.fypServerInstaller.ServiceName = "Service1";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.fypServerProcessInstaller,
            this.fypServerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller fypServerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller fypServerInstaller;
    }
}