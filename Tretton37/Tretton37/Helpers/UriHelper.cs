using System;

namespace Tretton37.Helpers
{
    /// <summary>
    /// Defines all the methods related to URI functionality.
    /// </summary>
    public sealed class UriHelper
    {
        public bool IsUri(string value)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}