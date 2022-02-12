using System;

namespace Tretton37.Helpers
{
    /// <summary>
    /// Defines methods related to the logging.
    /// </summary>
    internal class ConsoleLogHelper : ILogHelper
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}