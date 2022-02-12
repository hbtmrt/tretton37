using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tretton37.Core.Statics.Enums;
using Tretton37.Factories;

namespace Tretton37.Helpers
{
    /// <summary>
    /// Defines all the methods related to the HTML string manipulation.
    /// </summary>
    public sealed class HtmlFileHelper
    {
        internal void DownloadFiles(string uri, HtmlDocument document)
        {
            List<string> resoucesUris = GetResourcesUris(document);
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
