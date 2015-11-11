using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.ConnectedContent.Models
{
    [OrchardFeature("IDeliverable.Bits.ConnectedContent")]
    public class ContentConnectionRecord
    {
        public virtual int Id { get; set; }
        public virtual int ContentId { get; set; }
        public virtual int ConnectedContentId { get; set; }
        public virtual string GroupName { get; set; }
    }
}