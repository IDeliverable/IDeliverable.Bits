using System;
using System.Linq;
using System.Web;
using Orchard.UI;

namespace IDeliverable.Bits.Helpers
{
    public static class ZoneHelpers
    {
        public static bool ZoneHasContents(dynamic zoneShape)
        {
            var zone = zoneShape as Zone;
            if (zone != null)
            {
                foreach (var item in zone.Items)
                {
                    var itemIsNotTextField = item.Metadata != null && item.Metadata.Type != "Fields_Common_Text";
                    var itemIsWidget = item.Metadata != null && item.Metadata.Type == "Widget";
                    var itemHasValue = item.Value != null && !String.IsNullOrWhiteSpace(item.Value.ToString());
                    var itemIsTitlePart = item.Metadata != null && item.Metadata.Type == "Parts_Title";
                    var itemIsString = item is IHtmlString;

                    if (itemIsWidget)
                    {
                        var menuWidget = FindShape(item.Content, "Parts_MenuWidget");

                        if (menuWidget != null && menuWidget.Items.Count > 0)
                            return true;
                    }

                    if (itemIsTitlePart && item.ContentPart.IsHidden != null && item.ContentPart.IsHidden.Value != true)
                        return true;

                    if (itemIsString)
                    {
                        if (!String.IsNullOrWhiteSpace(((IHtmlString)item).ToString()))
                            return true;
                    }

                    if (itemIsNotTextField || itemHasValue)
                        return true;
                }
            }

            return false;
        }

        public static bool AnyZonesHaveContents(params dynamic[] zoneShapes)
        {
            var zonesQuery =
                from z in zoneShapes
                where ZoneHasContents(z)
                select z;

            return zonesQuery.Any();
        }

        public static dynamic FindShape(dynamic shape, string shapeType, string shapeName = null)
        {
            if (shape.Metadata.Type == shapeType && (shapeName == null || shape.Name == shapeName))
                return shape;

            foreach (var item in shape.Items)
            {
                var result = FindShape(item, shapeType, shapeName);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}