using Tretton37.Helpers;

namespace Tretton37.Factories
{
    /// <summary>
    /// Creates objects of type <see cref="ILogHelper"/>.
    /// </summary>
    public static class LoggerFactory
    {
        private static ILogHelper consoleLogHelper = new ConsoleLogHelper();
        public static ILogHelper CreateInstance()
        {
            // currently we have only writing to the console, but in future we can extend it to write any other tools.
            return consoleLogHelper;
        }
    }
}