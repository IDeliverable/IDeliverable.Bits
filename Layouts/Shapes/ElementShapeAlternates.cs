using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Elements;
using System;

namespace IDeliverable.Bits.Navigation.Shapes
{
    [OrchardFeature("IDeliverable.Bits.Layouts")]
    public class ElementShapeAlternates : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Element").OnDisplaying(context =>
            {
                var element = (Element)context.Shape.Element;
                var typeName = element.GetType().Name;

                if (element.Container != null)
                {
                    var containerTypeName = element.Container.GetType().Name;
                    context.ShapeMetadata.Alternates.Add($"Elements_{typeName}__Parent__{containerTypeName}");
                }
            });

            builder.Describe("Elements_Snippet_Field_Design").OnDisplaying(context =>
            {
                context.ShapeMetadata.Alternates.Add(String.Format("Elements_Snippet_Field_Design__{0}", context.Shape.Type));
            });
        }
    }
}