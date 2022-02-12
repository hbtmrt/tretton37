using System;

namespace Tretton37.Core.CustomExceptions
{
    /// <summary>
    /// A custom exception class to handle resource extraction exceptions.
    /// </summary>
    public class ResourceExtractionException : Exception
    {
        public ResourceExtractionException(string message) : base(message)
        {
        }
    }
}