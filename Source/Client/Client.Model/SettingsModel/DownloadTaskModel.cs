using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for downloading a <see cref="Task" />.
    /// </summary>
    public sealed class DownloadTaskModel : NotifiableModel
    {
        private const string FileExtension = ".zip";
        private readonly Task taskToDownload;
        private long downloadedSize;
        private string downloadLocation;
        private bool isDownloading;
        private long totalSize = 1;

        /// <summary>
        /// Create a new model for downloading a <see cref="Task" />.
        /// </summary>
        /// <param name="taskToDownload">The <see cref="Task" /> that will be downloaded.</param>
        public DownloadTaskModel(Task taskToDownload)
        {
            this.taskToDownload = taskToDownload;
        }

        public long DownloadedSize
        {
            get { return downloadedSize; }
            set
            {
                if (Equals(value, downloadedSize))
                {
                    return;
                }

                downloadedSize = value;

                OnPropertyChanged();
            }
        }

        public long TotalSize
        {
            get { return totalSize; }
            set
            {
                if (Equals(value, totalSize))
                {
                    return;
                }

                totalSize = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The name of the file to download.
        /// </summary>
        public string TaskFileName => $"{taskToDownload.BandId}_{taskToDownload.Id}_{taskToDownload.Title}_{taskToDownload.AssignedUserId}{FileExtension}";

        /// <summary>
        /// Is the <see cref="Task" /> currently downloading?
        /// </summary>
        public bool IsDownloading
        {
            get { return isDownloading; }
            set
            {
                if (Equals(value, isDownloading))
                {
                    return;
                }

                isDownloading = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The location to download the file to.
        /// </summary>
        public string DownloadLocation
        {
            get { return downloadLocation; }
            set
            {
                if (Equals(value, downloadLocation))
                {
                    return;
                }

                downloadLocation = value;

                OnPropertyChanged();
            }
        }
    }
}