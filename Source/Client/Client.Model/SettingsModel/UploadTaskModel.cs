using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for uploading a <see cref="Task" />.
    /// </summary>
    public sealed class UploadTaskModel : NotifiableModel
    {
        private readonly Task task;
        private string fileExtension;
        private string fileToUploadLocation;
        private bool isUploading;
        private long totalSize = 1;
        private long uploadSize;

        /// <summary>
        /// Create a new model for uploading a <see cref="Task" />.
        /// </summary>
        /// <param name="task">The <see cref="Task" /> that will be updated.</param>
        public UploadTaskModel(Task task)
        {
            this.task = task;
        }

        public long UploadSize
        {
            get { return uploadSize; }
            set
            {
                if (Equals(value, uploadSize))
                {
                    return;
                }

                uploadSize = value;

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
        /// The name of the file to upload.
        /// </summary>
        public string TaskFileName
        {
            get { return string.Format("{0}_{1}_{2}_{3}{4}", task.BandId, task.Id, task.Title, task.AssignedUserId, FileExtension); }
        }

        /// <summary>
        /// The Id of the <see cref="Task" /> to upload.
        /// </summary>
        public int TaskId
        {
            get { return task.Id; }
        }

        /// <summary>
        /// Is the <see cref="Task" /> currently uploading?
        /// </summary>
        public bool IsUploading
        {
            get { return isUploading; }
            set
            {
                if (Equals(value, isUploading))
                {
                    return;
                }

                isUploading = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The name of the file to upload.
        /// </summary>
        public string FileToUploadLocation
        {
            get { return fileToUploadLocation; }
            set
            {
                if (Equals(value, fileToUploadLocation))
                {
                    return;
                }

                fileToUploadLocation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The file extension.
        /// </summary>
        public string FileExtension
        {
            get { return fileExtension; }
            set
            {
                if (Equals(value, fileExtension))
                {
                    return;
                }

                fileExtension = value;

                OnPropertyChanged();
            }
        }
    }
}