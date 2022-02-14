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
    /// Makes a list of downloadable scripts in the HTML string.
    /// </summary>
    public sealed class ScriptResourceExtractor : IResourceExtractor
    {
        public List<Uri> Extract(Uri origin, HtmlDocument document)
        {
            try
            {
                UriHelper uriHelper = new UriHelper();
                return document.DocumentNode
                       .Descendants(Constants.DownloadableHtmlNodes.Script)
                       .Select(n => n.Attributes[Constants.HtmlAttributes.Src])
                       .Where(a => a != null
                            && !string.IsNullOrWhiteSpace(a.Value)
                            && !a.Value.Contains(Constants.Cdn)
                            && !a.Value.Contains("//")
                            && !uriHelper.IsUri(a.Value))
                       .Select(s => new Uri($"{origin.AbsoluteUri}/{s.Value}"))
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