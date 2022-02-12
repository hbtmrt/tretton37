using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tretton37.Core;
using Tretton37.Core.Statics.Enums;
using Tretton37.Factories;

namespace Tretton37.Helpers
{
    /// <summary>
    /// Defines all the methods related to the HTML string manipulation.
    /// </summary>
    public sealed class HtmlFileHelper
    {
        #region Declarations

        private static int downloadedCount = 0;
        private static int totalCount = 0;
        private static bool isDownloadingStarted = false;
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

        internal async Task DownloadFilesAsync(string uri, HtmlDocument document)
        {
            List<string> resoucesUris = GetResourcesUris(document);
            logHelper.Write(Constants.LogMessages.CompletedExtractingDownloadableFiles);

            totalCount = resoucesUris.Count();

            if (totalCount == 0)
            {
                logHelper.Write(Constants.LogMessages.NoItemsToDownload);
                return;
            }

            SetDownloadedCount(0);
            isDownloadingStarted = true;

            DeleteFileContainer();

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

        private void DeleteFileContainer()
        {
            string containerFilePath = $"{Environment.CurrentDirectory}//{Constants.FileContainerName}";
            if (Directory.Exists(containerFilePath))
            {
                Directory.Delete(containerFilePath, true);
            }
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

        private string FormatFilePath(string filePath)
        {
            if (filePath.Contains("?"))
            {
                return filePath.Split('?')[0];
            }

            return filePath;
        }

        private void CreateFolderPath(string path)
        {
            string directory = Path.GetDirectoryName(path); ;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private void SetDownloadedCount(int count)
        {
            downloadedCount = count;
            ShowDownloadingPercentage();
        }

        private void ShowDownloadingPercentage()
        {
            if (isDownloadingStarted)
            {
                // remove the last console.
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }

            logHelper.Write($"{Constants.Downloading} {DownloadingPercentage}%");
        }

        private List<string> GetResourcesUris(HtmlDocument document)
        {
            List<string> uris = new List<string>();

            foreach (DownloadableResourceTypes type in Enum.GetValues(typeof(DownloadableResourceTypes)))
            {
                uris.AddRange(ResourceExtractorFactory.CreateInstance(type).Extract(document));
            }

            return uris;
        }

        #endregion Methods - Helpers

        #endregion Methods
    }
}