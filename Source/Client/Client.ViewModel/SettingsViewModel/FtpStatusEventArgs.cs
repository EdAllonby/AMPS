using System;
using System.Net;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Encapsulates a <see cref="FtpStatusCode" /> into an event argument.
    /// </summary>
    public sealed class FtpStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Create the <see cref="FtpStatusCode" /> <see cref="EventArgs" /> with an <see cref="FtpStatusCode" />.
        /// </summary>
        /// <param name="statusCode">The <see cref="FtpStatusCode" /> to encapsulate.</param>
        public FtpStatusEventArgs(FtpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// The <see cref="FtpStatusCode" /> error.
        /// </summary>
        public FtpStatusCode StatusCode { get; }
    }
}