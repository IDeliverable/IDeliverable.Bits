using System.Collections.Generic;
using Orchard.ContentManagement;

namespace IDeliverable.Bits.Relationships.ViewModels
{
    public class RelatedByByViewModel
    {
        public IDictionary<string, IEnumerable<ContentItem>> RelatedContentItems { get; set; }
    }
}