using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace FileReadSampleApi.Services
{
    public class FileReader
    {

        static ConcurrentDictionary<string, Task<byte[]>> dict = new ConcurrentDictionary<string, Task<byte[]>>();

        /// <summary>
        /// Можем отрезать расширение, можем не приводить к нижнему регистру если у нас *nix
        /// все зависит от задачи и ОС
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string GetKey(string filename)
        {
            return filename.ToLower();
        }

        public static async Task<byte[]> ReadFileAync(string rootFolder, string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            string fullPath = Path.Combine(rootFolder, filename);
            if (!File.Exists(fullPath))
                throw new Exception($"file '{filename}' not found!");

            string key = GetKey(filename);

            Task<byte[]> task = GetDownloadTask(key, fullPath);

            return await task;
        }

        private static int EmulateWaitInMs = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;


        static object locker = new object();

        private static Task<byte[]> GetDownloadTask(string key, string fullPath)
        {
            Metrics.RequestsInc();
            Task<byte[]> task;

            if (dict.TryGetValue(key, out task))
            {
                return task;
            }
            else
            {
                // вариант реализации (lock + double check даст нам гарантии что никто не читает уже файл)
                lock (locker)
                {
                    if (dict.TryGetValue(key, out task))
                        return task;

                    task = File.ReadAllBytesAsync(fullPath)
                        .ContinueWith(
                        readTask =>
                        {
                            Metrics.FileReadsInc();
                            Task.Delay(EmulateWaitInMs).Wait();
                            return readTask.Result;
                        });
                    dict.TryAdd(key, task);
                }
            }
            return task;
        }
    }
}