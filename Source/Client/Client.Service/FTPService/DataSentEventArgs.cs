using System;

namespace Client.Service.FTPService
{
    public sealed class DataSentEventArgs : EventArgs
    {
        private readonly long bytesSent;
        private readonly long totalBytes;

        public DataSentEventArgs(long bytesSent, long totalBytes)
        {
            this.bytesSent = bytesSent;
            this.totalBytes = totalBytes;
        }

        public long BytesSent
        {
            get { return bytesSent; }
        }

        public long TotalBytes
        {
            get { return totalBytes; }
        }
    }
}