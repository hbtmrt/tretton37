using System;
using Tretton37.Core;

namespace Tretton37.Helpers
{
    /// <summary>
    /// Defines methods related to the arguments sent to the Main method.
    /// </summary>
    internal class ArgumentHelper
    {
        internal string GetWebsiteLink(string[] someArguments)
        {
            int firstArgIndex = 0;

            if (someArguments.Length == 0 ||
                string.IsNullOrWhiteSpace(someArguments[firstArgIndex]) ||
                !IsUri(someArguments[firstArgIndex]))
            {
                return Constants.DefaultUriString;
            }

            return someArguments[firstArgIndex];
        }

        private bool IsUri(string value)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}