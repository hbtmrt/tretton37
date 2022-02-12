using HtmlAgilityPack;
using System;
using System.Threading.Tasks;
using Tretton37.Core;
using Tretton37.Core.CustomExceptions;
using Tretton37.Helpers;

namespace Tretton37
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            LogHelper.Write(Constants.LogMessages.StartingProgram);
            string uri = new ArgumentHelper().GetWebsiteLink(args);

            // uri is never gonna empty, so we don't need to handle for null.
            try
            {
                HtmlDocument document = new HtmlWeb().Load(uri);
                LogHelper.Write(Constants.LogMessages.SourceDownloadCompleted);
                new HtmlFileHelper().DownloadFiles(uri, document);
            }
            catch (ResourceExtractionException ex)
            {
                LogHelper.Write(ex.Message);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
            }
        }
    }
}