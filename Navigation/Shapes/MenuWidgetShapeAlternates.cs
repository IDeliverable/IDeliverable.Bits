using System;
using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.Utility.Extensions;
using Orchard.Widgets.Models;

namespace IDeliverable.Bits.Navigation.Shapes
{
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class MenuWidgetShapeAlternates : IShapeTableProvider
    {
        public MenuWidgetShapeAlternates(Work<IMenuService> menuService)
        {
            mMenuService = menuService;
        }

        private readonly Work<IMenuService> mMenuService;

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Parts_MenuWidget").OnDisplaying(context =>
            {
                var contentItem = (ContentItem)context.Shape.ContentItem;
                var widgetPart = contentItem.As<WidgetPart>();
                var menuWidgetPart = contentItem.As<MenuWidgetPart>();
                var menuShape = context.Shape.Menu;
                var widgetName = widgetPart.Name.ToSafeName();
                var menu = mMenuService.Value.GetMenu(menuWidgetPart.MenuContentItemId);
                var menuName = menu.As<TitlePart>().Title.ToSafeName();
                var zoneName = widgetPart.Zone;

                if (!String.IsNullOrWhiteSpace(widgetName))
                    context.ShapeMetadata.Alternates.Add($"Parts_MenuWidget__WidgetName__{widgetName}");

                context.ShapeMetadata.Alternates.Add($"Parts_MenuWidget__MenuName__{menuName}");
                context.ShapeMetadata.Alternates.Add($"Parts_MenuWidget__Zone__{zoneName}");
                context.ShapeMetadata.Alternates.Add($"Parts_MenuWidget__Zone__{zoneName}__MenuName__{menuName}");

                menuShape.MenuWidget = contentItem;
                menuShape.SafeMenuName = menuName;
            });

            builder.Describe("Menu").OnDisplaying(ConfigureMenuShape);
            builder.Describe("Breadcrumb").OnDisplaying(ConfigureMenuShape);
        }

        private void ConfigureMenuShape(ShapeDisplayingContext context)
        {
            var widget = (ContentItem)context.Shape.MenuWidget;

            // Check if the Menu or Breadcrumb shape was created by a MenuWidget.
            if (widget == null)
                return;

            var widgetPart = widget.As<WidgetPart>();
            var menuName = (string)context.Shape.SafeMenuName;
            var widgetName = widgetPart.Name;
            var shapeType = context.ShapeMetadata.Type;

            if (!String.IsNullOrWhiteSpace(widgetName))
                context.ShapeMetadata.Alternates.Add($"{shapeType}__WidgetName__{widgetName}");

            context.ShapeMetadata.Alternates.Add($"{shapeType}__MenuName__{menuName}");

            ApplyRecursively(context.Shape.Items, (Action<dynamic>)(menuItemShape =>
            {
                var level = menuItemShape.Level;
                menuItemShape.Metadata.Alternates.Add($"MenuItem__Level{level}");

                if (!String.IsNullOrWhiteSpace(widgetName))
                {
                    menuItemShape.Metadata.Alternates.Add($"MenuItem__WidgetName__{widgetName}");
                    menuItemShape.Metadata.Alternates.Add($"MenuItem__WidgetName__{widgetName}__Level{level}");
                }

                menuItemShape.Metadata.Alternates.Add($"MenuItem__MenuName__{menuName}");
                menuItemShape.Metadata.Alternates.Add($"MenuItem__MenuName__{menuName}__Level{level}");
            }));
        }

        private void ApplyRecursively(IEnumerable<dynamic> menuItemShapes, Action<dynamic> action)
        {
            foreach (var menuItemShape in menuItemShapes)
            {
                action(menuItemShape);
                ApplyRecursively(menuItemShape.Items, action);
            }
        }
    }
}