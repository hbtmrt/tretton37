namespace Tretton37.Core
{
    /// <summary>
    /// Defines all the static values used throught the application.
    /// </summary>
    public static class Constants
    {
        public const string DefaultUriString = "https://tretton37.com/";

        public static class LogMessages
        {
            public const string StartingProgram = "The program is starting...";
            public const string SourceDownloadCompleted = "The HTML string has been downloaded.";
        }

        public static class DownloadableHtmlNodes
        {
            public const string Image = "img";
            public const string Link = "link";
        }

        public static class HtmlAttributes
        {
            public const string Src = "src";
            public const string Href = "href";
        }
    }
}