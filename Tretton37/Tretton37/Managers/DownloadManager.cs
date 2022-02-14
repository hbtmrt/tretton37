using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tretton37.Core;
using Tretton37.Factories;
using Tretton37.Helpers;

namespace Tretton37.Managers
{
    public sealed class DownloadManager
    {
        private readonly string uri;
        private readonly Queue<KeyValuePair<Uri, HtmlDocument>> uriQueue = new Queue<KeyValuePair<Uri, HtmlDocument>>();
        private readonly Queue<Uri> resourceUris = new Queue<Uri>();
        private readonly List<Uri> allExractedUris = new List<Uri>();
        private readonly object uriQueueSyncObject = new object();
        private readonly object resourceUriSyncObject = new object();
        private readonly HtmlWeb htmlWeb = new HtmlWeb();
        private readonly List<Task> downloadingTasks = new List<Task>();
        private readonly HtmlFileHelper htmlFileHelper = new HtmlFileHelper();
        private Uri origin;
        private readonly IOHelper ioHelper = new IOHelper();
        private int downloadedCount = 0;
        private bool isDownloadingStarted = false;
        private readonly ILogHelper logHelper = LoggerFactory.CreateInstance();
        private readonly UriHelper uriHelper = new UriHelper();
        private readonly List<string> unwantedUriKeys = new List<string>() { "mailto", "cookies", "javascript", "privacy-policy" };

        public int DownloadedPercentage
        {
            get
            {
                return allExractedUris.Count == 0 ? 0 : (int)Math.Floor((((decimal)downloadedCount) / allExractedUris.Count) * 100);
            }
        }

        public DownloadManager(string uri)
        {
            this.uri = uri;
        }

        internal async Task DownloadAsync()
        {
            // download all pages
            // iterate pages and return resource URIs
            // Download resources

            //Dictionary<string, HtmlDocument> pages = await new HtmlPageHelper().GetPagesAsync(uri);
            //List<string> resourceUris = await new HtmlFileHelper().GetResources(pages);
            // start downloading

            ioHelper.DeleteFileContainer();
            SetDownloadedCount(0);
            isDownloadingStarted = true;

            // link queue
            origin = new Uri(this.uri);
            uriQueue.Enqueue(new KeyValuePair<Uri, HtmlDocument>(origin, htmlWeb.Load(this.uri)));

            // TODO: move constant
            // Having too much async method might cause to slow program. Hence we stick to 10.
            int maximumNoOfTasks = 10;
            // create 10 async task
            for (int i = 0; i < maximumNoOfTasks; i++)
            {
                downloadingTasks.Add(ExtractUrisAsync());
            }

            for (int i = 0; i < maximumNoOfTasks; i++)
            {
                downloadingTasks.Add(DownloadResourcesAsync());
            }

            await Task.WhenAll(downloadingTasks);
        }

        private async Task DownloadResourcesAsync()
        {
            int retries = 0;

            while (retries < 5)
            {
                if (resourceUris.Count == 0)
                {
                    retries++;
                    Task.Delay(1000); // we give 5 excuses.
                }
                else
                {
                    Uri resourceUri;

                    lock (resourceUriSyncObject)
                    {
                        resourceUri = resourceUris.Dequeue();
                    }

                    ioHelper.CreateFolderPath(resourceUri.LocalPath);

                    await ioHelper.SaveFileAsync(resourceUri);
                    SetDownloadedCount(++downloadedCount);
                }
            }
        }

        private async Task ExtractUrisAsync()
        {
            int retries = 0;

            while (retries < 5)
            {
                KeyValuePair<Uri, HtmlDocument> uriKv = new KeyValuePair<Uri, HtmlDocument>();

                lock (uriQueueSyncObject)
                {
                    if (uriQueue.Count > 0)
                    {
                        uriKv = uriQueue.Dequeue();
                    }
                }

                if (uriKv.Equals(new KeyValuePair<Uri, HtmlDocument>()))
                {
                    retries++;
                    Task.Delay(1000); // we give 5 excuses.
                }
                else
                {
                    // get resource from Uris
                    //foreach (KeyValuePair<Uri, HtmlDocument> item in uris)
                    //{
                    //    htmlFileHelper.GetResourcesUris(item.Value);
                    //}

                    downloadingTasks.Add(SetPagesInDocumentAsync(uriKv.Value));

                    List<Uri> resources = htmlFileHelper.GetResources(origin, uriKv.Value);

                    lock (uriQueueSyncObject)
                    {
                        resources = resources.Where(r => !allExractedUris.Contains(r)).ToList();
                        allExractedUris.AddRange(resources);

                        resources.ForEach(r =>
                        {
                            resourceUris.Enqueue(r);
                        });

                        ShowDownloadingPercentage();
                    }

                    // Add total and downloaded count
                    // calculate percentage.
                }
            }

            // each task -
            // get 5 urls at a time
            // get the resources
            // download
        }

        private async Task SetPagesInDocumentAsync(HtmlDocument document)
        {
            List<string> pages = GetPagesInTheDocument(document);

                lock (uriQueueSyncObject)
                {
                    pages.ForEach(p =>
                    {
                        Uri newUri = new Uri(p);
                        uriQueue.Enqueue(new KeyValuePair<Uri, HtmlDocument>(newUri, htmlWeb.Load(newUri)));
                    });

                    pages.ForEach(p =>
                    {
                        downloadingTasks.Add(SetPagesInDocumentAsync(htmlWeb.Load(p)));
                    });
                }
        }

        private void SetDownloadedCount(int count)
        {
            downloadedCount = count;
            ShowDownloadingPercentage();
        }

        private void ShowDownloadingPercentage()
        {
            lock (resourceUriSyncObject)
            {
                if (isDownloadingStarted)
                {
                    // remove the last console.
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                }

                logHelper.Write($"{Constants.Downloading} {DownloadedPercentage}%");
            }
        }

        private List<string> GetPagesInTheDocument(HtmlDocument document)
        {
            List<string> pages = document.DocumentNode
                   .Descendants(Constants.DownloadableHtmlNodes.ATag)
                   .Select(n => n.Attributes[Constants.HtmlAttributes.Href])
                   .Where(a => a != null
                        && !string.IsNullOrWhiteSpace(a.Value)
                        && !uriHelper.IsUri(a.Value)
                        && !a.Value.Equals("/")
                        && !a.Value.StartsWith("#")
                        && !unwantedUriKeys.Contains(a.Value)
                        && !uriQueue.Select(q => q.Key).Contains(new Uri($"{this.uri}/{a.Value}")))
                   .Select(s => $"{origin}/{s.Value}")
                   .Distinct()
                   .ToList();

            return pages;
        }
    }
}