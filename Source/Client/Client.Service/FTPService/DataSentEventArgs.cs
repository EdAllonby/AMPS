using System;

namespace Client.Service.FTPService
{
    public sealed class DataSentEventArgs : EventArgs
    {
        public DataSentEventArgs(long bytesSent, long totalBytes)
        {
            this.BytesSent = bytesSent;
            this.TotalBytes = totalBytes;
        }

        public long BytesSent { get; }
        public long TotalBytes { get; }
    }
}