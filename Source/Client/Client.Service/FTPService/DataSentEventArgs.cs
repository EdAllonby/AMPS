using System;

namespace Client.Service.FTPService
{
    public sealed class DataSentEventArgs : EventArgs
    {
        public DataSentEventArgs(long bytesSent, long totalBytes)
        {
            BytesSent = bytesSent;
            TotalBytes = totalBytes;
        }

        public long BytesSent { get; }
        public long TotalBytes { get; }
    }
}