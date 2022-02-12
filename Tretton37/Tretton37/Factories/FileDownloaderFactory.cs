using Tretton37.Downloaders;

namespace Tretton37.Factories
{
    public static class FileDownloaderFactory
    {
        public static IFileDownloader CreateInstance()
        {
            // currently we have implemented Async downloader only.
            // in the future, we can implement parallel downloader or more downloaders.
            return new AsyncFileDownloader();
        }
    }
}