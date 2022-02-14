using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Tretton37.ResourceExtractors
{
    /// <summary>
    /// Defines methods to extract the downloadable resources from a HTML document.
    /// </summary>
    public interface IResourceExtractor
    {
        /// <summary>
        /// Extracts downloadable URIs from the document specified by <see cref="resourceType"/>.
        /// </summary>
        /// <param name="document">The HTML document</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>A list of downloadable resources.</returns>
        List<Uri> Extract(Uri origin, HtmlDocument document);
    }
}