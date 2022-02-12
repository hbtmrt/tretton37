using System;
using System.Collections.Generic;
using System.Text;

namespace Tretton37.Core.CustomExceptions
{
    public class ResourceExtractionException : Exception
    {
        public ResourceExtractionException(string message) : base(message)
        {
        }
    }
}
