using IDeliverable.Bits.Navigation.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Navigation.Drivers
{
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class ActionLinkPartDriver : ContentPartDriver<ActionLinkPart>
    {
        protected override string Prefix
        {
            get
            {
                return "ActionLink";
            }
        }

        protected override DriverResult Editor(ActionLinkPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ActionLink_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/ActionLink", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ActionLinkPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(ActionLinkPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("ActionName", part.ActionName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("ControllerName", part.ControllerName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("AreaName", part.AreaName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("RouteValues", part.RouteValues);
        }

        protected override void Importing(ActionLinkPart part, ImportContentContext context)
        {
            context.ImportAttribute(part.PartDefinition.Name, "ActionName", x => part.ActionName = x);
            context.ImportAttribute(part.PartDefinition.Name, "ControllerName", x => part.ControllerName = x);
            context.ImportAttribute(part.PartDefinition.Name, "AreaName", x => part.AreaName = x);
            context.ImportAttribute(part.PartDefinition.Name, "RouteValues", x => part.RouteValues = x);
        }
    }
}