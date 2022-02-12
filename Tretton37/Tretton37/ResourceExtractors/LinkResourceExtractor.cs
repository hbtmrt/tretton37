using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using Tretton37.Core;
using Tretton37.Core.CustomExceptions;
using Tretton37.Helpers;

namespace Tretton37.ResourceExtractors
{
    /// <summary>
    /// Makes a list of downloadable links in the HTML string.
    /// </summary>
    public sealed class LinkResourceExtractor : IResourceExtractor
    {
        public List<string> Extract(HtmlDocument document)
        {
            try
            {
                UriHelper uriHelper = new UriHelper();
                return document.DocumentNode
                       .Descendants(Constants.DownloadableHtmlNodes.Link)
                       .Select(n => n.Attributes[Constants.HtmlAttributes.Href])
                       .Where(a => a != null
                            && !string.IsNullOrWhiteSpace(a.Value)
                            && !a.Value.Contains("//")
                            && !uriHelper.IsUri(a.Value))
                       .Select(s => s.Value)
                       .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceExtractionException(ex.Message);
            }
        }
    }
}