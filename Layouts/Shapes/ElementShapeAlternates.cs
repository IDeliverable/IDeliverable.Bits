using System;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Elements;

namespace IDeliverable.Bits.Navigation.Shapes {
    [OrchardFeature("IDeliverable.Bits.Layouts")]
    public class ElementShapeAlternates : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Element").OnDisplaying(context =>
            {
                var element = (Element)context.Shape.Element;
                var typeName = element.GetType().Name;

                if (element.Container != null) {
                    var containerTypeName = element.Container.GetType().Name;
                    context.ShapeMetadata.Alternates.Add($"Elements_{typeName}__Parent__{containerTypeName}");
                }

                if(!String.IsNullOrWhiteSpace(element.HtmlId))
                {
                    context.ShapeMetadata.Alternates.Add($"Elements_{typeName}__{element.HtmlId}");
                    context.ShapeMetadata.Alternates.Add($"Elements__{element.HtmlId}");
                }
            });
        }
    }
}
