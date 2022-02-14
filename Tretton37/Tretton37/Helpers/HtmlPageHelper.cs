using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tretton37.Core;

namespace Tretton37.Helpers
{
    public sealed class HtmlPageHelper
    {
        private readonly List<string> unwantedUriKeys = new List<string>() { "mailto", "cookies", "javascript", "privacy-policy" };

        //private readonly Dictionary<string, HtmlDocument> extractedPages = new Dictionary<string, HtmlDocument>();
        private readonly UriHelper uriHelper = new UriHelper();

        private object syncLock = new object();
        private string origin = string.Empty;
        private readonly HtmlWeb htmlWeb = new HtmlWeb();
        private List<Task> pagesTasks = new List<Task>();
        private Dictionary<string, HtmlDocument> pagesDictionary = new Dictionary<string, HtmlDocument>();

        internal async Task<Dictionary<string, HtmlDocument>> GetPagesAsync(string uri)
        {
            origin = uri;
            HtmlDocument document = htmlWeb.Load(uri);
            pagesDictionary.Add(uri, document);

            pagesTasks.Add(GetPagesInDocumentAsync(document));

            await Task.WhenAll(pagesTasks);
            return pagesDictionary;
        }

        private async Task GetPagesInDocumentAsync(HtmlDocument document)
        {
            if (pagesDictionary.Count > 50) return;

            List<string> pages = GetPagesInTheDocument(document);

            pages.ForEach(p =>
            {
                lock (syncLock)
                {
                    pagesDictionary[p] = htmlWeb.Load(p);
                }
            });

            //int NoOfTasks = (int)Math.Ceiling(((double)pages.Count) / Constants.ProcessesPerTask);

            //for (int i = 0; i < NoOfTasks; i++)
            //{
            //    int min = i * Constants.ProcessesPerTask;
            //    int itemCount = (i + 1) * Constants.ProcessesPerTask > pages.Count ?
            //        pages.Count - (i + 1) * Constants.ProcessesPerTask :
            //        (i + 1) * Constants.ProcessesPerTask - 1;

            //    pages.GetRange(min, itemCount);
            //}

            pages.ForEach(p =>
            {
                pagesTasks.Add(GetPagesInDocumentAsync(pagesDictionary[p]));
            });
        }

        private List<string> GetPagesInTheDocument(HtmlDocument document)
        {
            return document.DocumentNode
                   .Descendants(Constants.DownloadableHtmlNodes.ATag)
                   .Select(n => n.Attributes[Constants.HtmlAttributes.Href])
                   .Where(a => a != null
                        && !string.IsNullOrWhiteSpace(a.Value)
                        && !uriHelper.IsUri(a.Value)
                        && !a.Value.Equals("/")
                        && !a.Value.StartsWith("#")
                        && !unwantedUriKeys.Contains(a.Value)
                        && !pagesDictionary.ContainsKey($"{origin}/{a.Value}"))
                   .Select(s => $"{origin}/{s.Value}")
                   .Distinct()
                   .ToList();
        }
    }
}