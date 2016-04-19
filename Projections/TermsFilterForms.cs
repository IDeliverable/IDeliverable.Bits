using System;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;

namespace IDeliverable.Bits.Projections
{
    [OrchardFeature("IDeliverable.Bits.Projections")]
    public class TermsFilterForms : Component, IFormProvider
    {
        public void Describe(DescribeContext context)
        {
            Func<IShapeFactory, object> form = shapeFactory =>
            {
                var shape = (dynamic)shapeFactory;
                var formShape = shape.Form
                (
                    Id: "Terms",
                    _Terms: shape.TextBox
                    (
                        Id: "termids", Name: "TermIds",
                        Title: T("Terms"),
                        Description: T("Enter one or more term IDs, seperated by a comma. Tip: you can use tokens such as #{Request.CurrentContent.Fields.MyTaxonomyField.Terms:0} to get the terms for the currently routed content item."),
                        Classes: "text large tokenized"
                    ),
                    _Exclusion: shape.FieldSet
                    (
                        _OperatorOneOf: shape.Radio
                        (
                            Id: "operator-is-one-of", Name: "Operator",
                            Title: T("Is one of"), Value: "0", Checked: true
                        ),
                        _OperatorIsAllOf: shape.Radio
                        (
                            Id: "operator-is-all-of", Name: "Operator",
                            Title: T("Is all of"), Value: "1"
                        )
                    )
                );

                return formShape;
            };

            context.Form("EnterTerms", form);
        }
    }
}