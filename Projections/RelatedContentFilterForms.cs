using System;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;

namespace IDeliverable.Bits.Projections
{
    [OrchardFeature("IDeliverable.Bits.Projections")]
    public class RelatedContentFilterForms : Component, IFormProvider
    {
        public void Describe(DescribeContext context)
        {
            Func<IShapeFactory, object> form = shapeFactory =>
            {
                var shape = (dynamic)shapeFactory;
                var formShape = shape.Form
                (
                    Id: "TaxonomyField",
                    _Terms: shape.TextBox
                    (
                        Id: "taxonomyFieldExpression", Name: "TaxonomyFieldExpression",
                        Title: T("Taxonomy Field Expression"),
                        Description: T("Enter the name of the taxonomy field. If there are more than one part contaiing a field with the same name, specify the path to the field, e.g. MyPart.MyField."),
                        Classes: "text large"
                    )
                );

                return formShape;
            };

            context.Form("RelatedContent", form);
        }
    }
}