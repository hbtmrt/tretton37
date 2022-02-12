using System;
using Tretton37.Core;
using Tretton37.Helpers;

namespace Tretton37
{
    class Program
    {
        private static readonly LogHelper logHelper = new LogHelper();

        static void Main(string[] args)
        {
            logHelper.Write(Constants.LogMessages.StartingProgram);
            string uri = new ArgumentHelper().GetWebsiteLink(args);
        }
    }
}
