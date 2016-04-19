using System;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;

namespace IDeliverable.Bits.Projections
{
    [OrchardFeature("IDeliverable.Bits.Projections")]
    public class ExclusionFilterForms : Component, IFormProvider
    {
        public void Describe(DescribeContext context)
        {
            Func<IShapeFactory, object> form = shapeFactory =>
            {
                var shape = (dynamic)shapeFactory;
                var formShape = shape.Form
                (
                    Id: "ExcludeContentItems",
                    _Terms: shape.TextBox
                    (
                        Id: "contentItemIds", Name: "ContentItemIds",
                        Title: T("Terms"),
                        Description: T("Enter one or more content item IDs, seperated by a comma. Tip: you can use tokens such as #{Request.CurrentContent.Id} to get the ID for the currently routed content item."),
                        Classes: "text large tokenized"
                    )
                );

                return formShape;
            };

            context.Form("ExcludeContentItems", form);
        }
    }
}