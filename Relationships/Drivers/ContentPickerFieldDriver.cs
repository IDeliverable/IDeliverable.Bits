using System.Linq;
using IDeliverable.Bits.Relationships.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentPicker.Fields;
using Orchard.ContentPicker.Settings;
using Orchard.ContentPicker.ViewModels;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using ContentItemElement = Orchard.Layouts.Elements.ContentItem;

namespace IDeliverable.Bits.Relationships.Drivers
{
    [OrchardFeature("IDeliverable.Bits.Relationships")]
    public class ContentPickerFieldDriver : ContentFieldDriver<ContentPickerField>
    {
        private readonly IRelationshipService mRelationshipService;

        public ContentPickerFieldDriver(IRelationshipService relationshipService)
        {
            mRelationshipService = relationshipService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentPickerField field, ContentPart part) {
            return part.PartDefinition.Name + "." + field.Name;
        }

        protected override DriverResult Editor(ContentPart part, ContentPickerField field, dynamic shapeHelper)
        {
            return null;
        }

        protected override DriverResult Editor(ContentPart part, ContentPickerField field, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = new ContentPickerFieldViewModel { SelectedIds = ContentItemElement.Serialize(field.Ids) };
            updater.TryUpdateModel(model, GetPrefix(field, part), null, null);

            var settings = field.PartFieldDefinition.Settings.GetModel<ContentPickerFieldSettings>();
            var ids = ContentItemElement.Deserialize(model.SelectedIds).ToArray();
            
            if (!settings.Required || ids.Length > 0)
            {
                mRelationshipService.SetRelationships(part.Id, field.Name, ids);
            }

            return null;
        }

        protected override void Importing(ContentPart part, ContentPickerField field, ImportContentContext context)
        {
            var contentItemIds = context.Attribute(field.FieldDefinition.Name + "." + field.Name, "ContentItems");
            if (contentItemIds != null)
            {
                var ids = contentItemIds.Split(',')
                    .Select(context.GetItemFromSession)
                    .Select(contentItem => contentItem.Id).ToArray();

                mRelationshipService.SetRelationships(part.Id, field.Name, ids);
            }
        }
    }
}