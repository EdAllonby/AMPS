using System;
using System.Windows;

namespace Client.View
{
    /// <summary>
    /// Helper for closing the application from a window close.
    /// </summary>
    public sealed class ApplicationExitHelper
    {
        /// <summary>
        /// Create a new helper for closing the application.
        /// </summary>
        public ApplicationExitHelper()
        {
            CanExitApplication = true;
        }

        /// <summary>
        /// Set a flag for whether the application can close.
        /// </summary>
        public bool CanExitApplication { private get; set; }

        /// <summary>
        /// Attempts to exit the application, based on the <see cref="CanExitApplication" /> state.
        /// </summary>
        public void TryExitApplication()
        {
            if (CanExitApplication)
            {
                Application.Current.Shutdown();

                // Something is stopping the application to close gracefully, let's force the application to quit for now.
                Environment.Exit(0);
            }
        }
    }
}