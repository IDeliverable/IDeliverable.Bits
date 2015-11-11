using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;

namespace IDeliverable.Bits.Relationships.Services
{
    public interface IRelationshipService : IDependency
    {
        IEnumerable<T> GetRelatedContentItems<T>(int contentId, VersionOptions options, QueryHints hints, string discriminator = null) where T : class, IContent;
        IDictionary<string, IEnumerable<T>> GetRelatedByContentItems<T>(int contentId, VersionOptions options, QueryHints hints) where T : class, IContent;
        void SetRelationships(int contentItemId, string discriminator, IEnumerable<int> relatedContentItemIds);
        void BreakRelationships(int contentItemId, string discriminator = null);
    }
}