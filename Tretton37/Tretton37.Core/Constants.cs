﻿namespace Tretton37.Core
{
    /// <summary>
    /// Defines all the static values used throught the application.
    /// </summary>
    public static class Constants
    {
        public const string DefaultUriString = "https://tretton37.com/";
        public const string Cdn = "cdn.";
        public const string Downloading = "Downloading...";

        public static class LogMessages
        {
            public const string StartingProgram = "The program is starting...";
            public const string SourceDownloadCompleted = "The HTML string has been downloaded.";
            public const string CompletedExtractingDownloadableFiles = "Completed extracting downloadable files.";
            public const string NoItemsToDownload = "There are no files to download.";
        }

        public static class DownloadableHtmlNodes
        {
            public const string Image = "img";
            public const string Link = "link";
            public const string Script = "script";
        }

        public static class HtmlAttributes
        {
            public const string Src = "src";
            public const string Href = "href";
        }
    }
}