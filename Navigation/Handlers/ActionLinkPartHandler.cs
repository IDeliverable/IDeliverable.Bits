using IDeliverable.Bits.Navigation.Models;
using IDeliverable.Bits.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Navigation.Handlers
{
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class ActionLinkPartHandler : ContentHandler
    {
        public ActionLinkPartHandler(IRouteValuesProcessor routeValuesProcessor)
        {
            mRouteValuesProcessor = routeValuesProcessor;
            OnActivated<ActionLinkPart>(SetupFieldHandlers);
        }

        private readonly IRouteValuesProcessor mRouteValuesProcessor;

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "ActionLink")
                return;

            var actionLinkPart = context.ContentItem.As<ActionLinkPart>();
            context.Metadata.DisplayRouteValues = actionLinkPart.RouteValueDictionary;
        }

        private void SetupFieldHandlers(ActivatedContentContext context, ActionLinkPart part)
        {
            part.RouteValueDictionaryField.Loader(() =>
            {
                var routeValues = mRouteValuesProcessor.Parse(part.RouteValues);

                routeValues["area"] = part.AreaName;
                routeValues["controller"] = part.ControllerName;
                routeValues["action"] = part.ActionName;

                return routeValues;
            });
        }
    }
}