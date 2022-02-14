using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tretton37.Downloaders
{
    /// <summary>
    /// File downloader interface.
    /// </summary>
    public interface IFileDownloader
    {
        Task DownloadAsync(string uri, List<string> resoucesUris);
    }
}