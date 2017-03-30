using System.IO;

namespace Client.ViewModel.MainViewModel
{
    internal static class AudioUtilities
    {
        public static byte[] GetBytesFromWavFile(string filename)
        {
            byte[] audio;

            using (FileStream fileStream = File.OpenRead(filename))
            {
                audio = new byte[fileStream.Length];

                var currentBytesRead = 0;

                var numberOfBytesToRead = (int) fileStream.Length;

                while (numberOfBytesToRead > 0)
                {
                    int readBytes = fileStream.Read(audio, currentBytesRead, numberOfBytesToRead);

                    if (readBytes == 0)
                    {
                        break;
                    }

                    numberOfBytesToRead -= readBytes;
                    currentBytesRead += readBytes;
                }

                fileStream.Close();
            }

            return audio;
        }
    }
}