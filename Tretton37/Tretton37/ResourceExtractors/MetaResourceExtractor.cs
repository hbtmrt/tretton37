using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using Tretton37.Core;
using Tretton37.Core.CustomExceptions;
using Tretton37.Helpers;

namespace Tretton37.ResourceExtractors
{
    public sealed class MetaResourceExtractor : IResourceExtractor
    {
        public List<Uri> Extract(Uri origin, HtmlDocument document)
        {
            try
            {
                UriHelper uriHelper = new UriHelper();
                return document.DocumentNode
                       .Descendants(Constants.DownloadableHtmlNodes.Meta)
                       .Select(n => n.Attributes[Constants.HtmlAttributes.Content])
                       .Where(a => a != null
                            && !string.IsNullOrWhiteSpace(a.Value)
                            && uriHelper.IsUri(a.Value))
                       .Select(s => new Uri(s.Value))
                       .Distinct()
                       .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceExtractionException(ex.Message);
            }
        }
    }
}