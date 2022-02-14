using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Tretton37.Core;
using Tretton37.Factories;
using Tretton37.Helpers;

namespace Tretton37.Downloaders
{
    /// <summary>
    /// Files are downloaded using asynchronous way.
    /// </summary>
    public sealed class AsyncFileDownloader : IFileDownloader
    {
        #region Declarations

        private static int downloadedCount = 0;
        private static int totalCount = 0;
        private static bool isDownloadingStarted = false;
        private static object syncLock = new object();
        private readonly ILogHelper logHelper = LoggerFactory.CreateInstance();

        #endregion Declarations

        #region Properties

        public int DownloadingPercentage
        {
            get
            {
                return totalCount == 0 ? 0 : (int)Math.Floor((((decimal)downloadedCount) / totalCount) * 100);
            }
        }

        #endregion Properties

        #region Methods

        #region Methods - Instance Members

        public async Task DownloadAsync(string uri, List<string> resoucesUris)
        {
            totalCount = resoucesUris.Count;
            DeleteFileContainer();

            SetDownloadedCount(0);
            isDownloadingStarted = true;

            List<Task> tasks = new List<Task>();
            //// Start downloading
            int countFraction = (int)Math.Ceiling(((double)totalCount) / Constants.NoOfAsyncProcesses);
            for (int i = 0; i < Constants.NoOfAsyncProcesses; i++)
            {
                int min = i * countFraction;
                int itemCount = (i + 1) * countFraction > totalCount ? totalCount - i * countFraction : countFraction;
                Task task = SaveFilesAsync(uri, resoucesUris.GetRange(min, itemCount));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        #endregion Methods - Instance Members

        #region Methods - Helpers

        private void SetDownloadedCount(int count)
        {
            downloadedCount = count;
            ShowDownloadingPercentage();
        }

        private async Task SaveFilesAsync(string uri, List<string> filePathList)
        {
            await Task.Factory.StartNew(async () =>
            {
                foreach (var filePath in filePathList)
                {
                    await SaveFileAsync(uri, filePath);
                }
            });
        }

        private async Task SaveFileAsync(string uri, string filePath)
        {
            string fullUri = $"{uri}{filePath}";
            string containerFilePath = $"{Environment.CurrentDirectory}//{Constants.FileContainerName}";
            string localFilePath = $"{containerFilePath}/{FormatFilePath(filePath)}";

            CreateFolderPath(localFilePath);

            using var client = new WebClient();
            client.DownloadFile(fullUri, localFilePath);

            SetDownloadedCount(++downloadedCount);
        }

        private void ShowDownloadingPercentage()
        {
            lock (syncLock)
            {
                if (isDownloadingStarted)
                {
                    // remove the last console.
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                }

                logHelper.Write($"{Constants.Downloading} {DownloadingPercentage}%");
            }
        }

        private void CreateFolderPath(string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string FormatFilePath(string filePath)
        {
            if (filePath.Contains("?"))
            {
                return filePath.Split('?')[0];
            }

            return filePath;
        }

        private void DeleteFileContainer()
        {
            string containerFilePath = $"{Environment.CurrentDirectory}//{Constants.FileContainerName}";
            if (Directory.Exists(containerFilePath))
            {
                Directory.Delete(containerFilePath, true);
            }
        }

        #endregion Methods - Helpers

        #endregion Methods
    }
}