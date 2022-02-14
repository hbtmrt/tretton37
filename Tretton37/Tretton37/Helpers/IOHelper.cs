using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tretton37.Core;

namespace Tretton37.Helpers
{
    public sealed class IOHelper
    {
        private readonly string containerFilePath = $"{Environment.CurrentDirectory}//{Constants.FileContainerName}";

        internal async Task SaveFileAsync(Uri uri)
        {
            string localFilePath = $"{containerFilePath}/{uri.LocalPath}";

            using var client = new WebClient();
            client.DownloadFileAsync(uri, localFilePath);
        }

        internal void DeleteFileContainer()
        {
            if (Directory.Exists(containerFilePath))
            {
                Directory.Delete(containerFilePath, true);
            }
        }

        internal void CreateFolderPath(string localPath)
        {
            string path = $"{Environment.CurrentDirectory}//{Constants.FileContainerName}//{localPath}";
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
}
