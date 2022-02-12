using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
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
            List<string> resoucesUris = GetResourcesUris(document);
            logHelper.Write(Constants.LogMessages.CompletedExtractingDownloadableFiles);

            int totalCount = resoucesUris.Count();

            if (totalCount == 0)
            {
                logHelper.Write(Constants.LogMessages.NoItemsToDownload);
                return;
            }

            await FileDownloaderFactory.CreateInstance().Download(uri, resoucesUris);
        }

        #endregion Methods - Instance Members

        #region Methods - Helpers

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