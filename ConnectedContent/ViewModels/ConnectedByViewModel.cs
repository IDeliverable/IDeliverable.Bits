using System.Collections.Generic;
using Orchard.ContentManagement;

namespace IDeliverable.Bits.ConnectedContent.ViewModels
{
    public class ConnectedByViewModel
    {
        public IDictionary<string, IEnumerable<ContentItem>> RelatedContentItems { get; set; }
    }
}