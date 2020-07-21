using System;
using System.Threading;

namespace FileReadSampleApi.Services
{
    /// <summary>
    /// В реальной жизни прометей + графана
    /// </summary>
    public static class Metrics
    {
        public static int FileReads;
        public static int Requests;

        public static void FileReadsInc()
        {
            Interlocked.Increment(ref Metrics.FileReads);
        }

        public static void RequestsInc()
        {
            Interlocked.Increment(ref Metrics.Requests);
        }
    }
}
