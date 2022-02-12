using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Tretton37.Core.Statics.Enums;

namespace Tretton37.ResourceExtractors
{
    /// <summary>
    /// Makes a list of downloadable scripts in the HTML string.
    /// </summary>
    public sealed class ScriptResourceExtractor : IResourceExtractor
    {
        public List<string> Extract(HtmlDocument document)
        {
            return new List<string>();
        }
    }
}
