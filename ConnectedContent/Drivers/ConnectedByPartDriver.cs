using System.Linq;
using IDeliverable.Bits.ConnectedContent.Parts;
using IDeliverable.Bits.ConnectedContent.Services;
using IDeliverable.Bits.ConnectedContent.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.ConnectedContent.Drivers
{
    [OrchardFeature("IDeliverable.Bits.ConnectedContent")]
    public class ConnectedByPartDriver : ContentPartDriver<ConnectedByPart>
    {
        private readonly IContentConnectionService mContentConnectionService;

        public ConnectedByPartDriver(IContentConnectionService contentConnectionService)
        {
            mContentConnectionService = contentConnectionService;
        }

        protected override DriverResult Editor(ConnectedByPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ConnectedBy_Edit", () =>
            {
                var relatedContentItems = mContentConnectionService.GetConnectedByItems<ContentItem>(part.Id, VersionOptions.Latest, QueryHints.Empty);
                var viewModel = new ConnectedByViewModel
                {
                    RelatedContentItems = relatedContentItems
                };
                return shapeHelper.EditorTemplate(TemplateName: "Parts/ConnectedBy", Model: viewModel, Prefix: Prefix);
            });
        }
    }
}