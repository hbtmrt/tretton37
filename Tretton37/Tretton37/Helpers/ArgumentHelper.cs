using System;
using Tretton37.Core;

namespace Tretton37.Helpers
{
    /// <summary>
    /// Defines methods related to the arguments sent to the Main method.
    /// </summary>
    internal sealed class ArgumentHelper
    {
        internal string GetWebsiteLink(string[] someArguments)
        {
            int firstArgIndex = 0;

            if (someArguments.Length == 0 ||
                string.IsNullOrWhiteSpace(someArguments[firstArgIndex]) ||
                !new UriHelper().IsUri(someArguments[firstArgIndex]))
            {
                return Constants.DefaultUriString;
            }

            return someArguments[firstArgIndex];
        }
    }
}