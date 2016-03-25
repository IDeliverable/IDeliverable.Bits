using Orchard;
using Orchard.ContentManagement;

namespace IDeliverable.Bits.Services
{
    public interface ICurrentContentAccessor : IDependency
    {
        ContentItem CurrentContentItem { get; }
    }
}