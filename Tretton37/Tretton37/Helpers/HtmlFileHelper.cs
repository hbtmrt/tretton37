using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static int downloadedCount = 0;
        private static int totalCount = 0;
        private bool isDownloadingStarted = false;

        public int DownloadingPercentage
        {
            get
            {
                return totalCount == 0 ? 0 : (int)Math.Floor((((decimal)downloadedCount) / totalCount) * 100);
            }
        }

        internal void DownloadFiles(string uri, HtmlDocument document)
        {
            List<string> resoucesUris = GetResourcesUris(document);
            LogHelper.Write(Constants.LogMessages.CompletedExtractingDownloadableFiles);

            totalCount = resoucesUris.Count();

            if (totalCount == 0)
            {
                LogHelper.Write(Constants.LogMessages.NoItemsToDownload);
                return;
            }

            SetDownloadedCount(0);

            //// Start downloading

            //downloadedCount = 4;
            //ShowDownloadingPercentage();
        }

        private void SetDownloadedCount(int count)
        {
            if (!isDownloadingStarted)
            {
                isDownloadingStarted = true;
            }

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

            LogHelper.Write($"{Constants.Downloading} {DownloadingPercentage}%");
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
    }
}