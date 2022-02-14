using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly ILogHelper logHelper = LoggerFactory.CreateInstance();

        #endregion Declarations

        #region Methods

        #region Methods - Instance Members

        internal async Task DownloadFilesAsync(string uri, HtmlDocument document)
        {
            //logHelper.Write(Constants.LogMessages.GatheringInformation);
            //List<string> resoucesUris = GetResources(document);
            //logHelper.Write(Constants.LogMessages.CompletedExtractingDownloadableFiles);

            //int totalCount = resoucesUris.Count();

            //if (totalCount == 0)
            //{
            //    logHelper.Write(Constants.LogMessages.NoItemsToDownload);
            //    return;
            //}

            //await FileDownloaderFactory.CreateInstance().DownloadAsync(uri, resoucesUris);
        }

        #endregion Methods - Instance Members

        #region Methods - Helpers

        public List<Uri> GetResources(Uri origin, HtmlDocument document)
        {
            List<Uri> uris = new List<Uri>();

            foreach (DownloadableResourceTypes type in Enum.GetValues(typeof(DownloadableResourceTypes)))
            {
                uris.AddRange(ResourceExtractorFactory.CreateInstance(type).Extract(origin, document));
            }

            // Filter to have unique values since diffrent pages may have same resource.
            return uris.Distinct().ToList();
        }

        public List<Uri> GetResourceUris(Uri origin, List<HtmlDocument> documents)
        {
            List<Uri> uris = new List<Uri>();

            documents.ForEach(document => {
                uris.AddRange(GetResources(origin, document));
            });

            return uris;
        }

        #endregion Methods - Helpers

        #endregion Methods
    }
}