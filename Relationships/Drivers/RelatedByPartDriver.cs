using IDeliverable.Bits.Relationships.Parts;
using IDeliverable.Bits.Relationships.Services;
using IDeliverable.Bits.Relationships.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Relationships.Drivers
{
    [OrchardFeature("IDeliverable.Bits.Relationships")]
    public class RelatedByPartDriver : ContentPartDriver<RelatedByPart>
    {
        private readonly IRelationshipService mRelationshipService;

        public RelatedByPartDriver(IRelationshipService relationshipService)
        {
            mRelationshipService = relationshipService;
        }

        protected override DriverResult Editor(RelatedByPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_RelatedBy_Edit", () =>
            {
                var relatedContentItems = mRelationshipService.GetRelatedByContentItems<ContentItem>(part.Id, VersionOptions.Latest, QueryHints.Empty);
                var viewModel = new RelatedByByViewModel
                {
                    RelatedContentItems = relatedContentItems
                };
                return shapeHelper.EditorTemplate(TemplateName: "Parts/RelatedBy", Model: viewModel, Prefix: Prefix);
            });
        }
    }
}