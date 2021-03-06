using Tretton37.Core.Statics.Enums;
using Tretton37.ResourceExtractors;

namespace Tretton37.Factories
{
    /// <summary>
    /// A factory to produce objects of type <see cref="IResourceExtractor"/>.
    /// </summary>
    public static class ResourceExtractorFactory
    {
        public static IResourceExtractor CreateInstance(DownloadableResourceTypes resourceType) =>
            resourceType switch
            {
                DownloadableResourceTypes.Image => new ImageResourceExtractor(),
                DownloadableResourceTypes.Links => new LinkResourceExtractor(),
                DownloadableResourceTypes.Meta => new MetaResourceExtractor(),
                _ => new ScriptResourceExtractor()
            };
    }
}