using HtmlAgilityPack;
using System;
using System.Threading.Tasks;
using Tretton37.Core;
using Tretton37.Core.CustomExceptions;
using Tretton37.Factories;
using Tretton37.Helpers;

namespace Tretton37
{
    internal class Program
    {
        private static ILogHelper logHelper = LoggerFactory.CreateInstance();

        private static async Task Main(string[] args)
        {
            logHelper.Write(Constants.LogMessages.StartingProgram);
            string uri = new ArgumentHelper().GetWebsiteLink(args);

            // uri is never gonna empty, so we don't need to handle for null.
            try
            {
                HtmlDocument document = new HtmlWeb().Load(uri);
                logHelper.Write(Constants.LogMessages.SourceDownloadCompleted);
                await new HtmlFileHelper().DownloadFilesAsync(uri, document);
                logHelper.Write(Constants.LogMessages.DownloadingFilesCompleted);
            }
            catch (ResourceExtractionException ex)
            {
                logHelper.Write(ex.Message);
            }
            catch (Exception ex)
            {
                logHelper.Write(ex.Message);
            }
            finally
            {
                logHelper.Write(Constants.LogMessages.ExcecutingFinished);
            }
        }
    }
}