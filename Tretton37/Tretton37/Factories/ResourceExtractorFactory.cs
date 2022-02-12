using Tretton37.Core.Statics.Enums;
using Tretton37.ResourceExtractors;

namespace Tretton37.Factories
{
    public static class ResourceExtractorFactory
    {
        public static IResourceExtractor CreateInstance(DownloadableResourceTypes resourceType) =>
            resourceType switch
            {
                DownloadableResourceTypes.Image => new ImageResourceExtractor(),
                DownloadableResourceTypes.Links => new LinkResourceExtractor(),
                _ => new ScriptResourceExtractor()
            };
    }
}