using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Relationships.Models
{
    [OrchardFeature("IDeliverable.Bits.Relationships")]
    public class RelationshipRecord
    {
        public virtual int Id { get; set; }
        public virtual int ContentItemId { get; set; }
        public virtual int RelatedContentItemId { get; set; }
        public virtual string Discriminator { get; set; }
    }
}