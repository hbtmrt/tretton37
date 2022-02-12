using HtmlAgilityPack;
using System;
using Tretton37.Core;
using Tretton37.Helpers;

namespace Tretton37
{
    internal class Program
    {
        private static readonly LogHelper logHelper = new LogHelper();

        private static void Main(string[] args)
        {
            logHelper.Write(Constants.LogMessages.StartingProgram);
            string uri = new ArgumentHelper().GetWebsiteLink(args);

            // uri is never gonna empty, so we don't need to handle for null.
            try
            {
                var document = new HtmlWeb().Load(uri);
            }
            catch (Exception ex)
            {
                logHelper.Write(ex.Message);
            }
        }
    }
}