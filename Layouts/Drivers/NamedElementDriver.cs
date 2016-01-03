using System;
using System.Collections.Generic;
using IDeliverable.Bits.Layouts.Helpers;
using IDeliverable.Bits.Layouts.ViewModels;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;
using Orchard.Layouts.Framework.Elements;

namespace IDeliverable.Bits.Layouts.Drivers {
    [OrchardFeature("IDeliverable.Bits.Layouts")]
    public class NamedElementDriver : ElementDriver<Element> {
        protected override EditorResult OnBuildEditor(Element element, ElementEditorContext context) {
            var viewModel = new NamedElementViewModel {
                Name = element.GetName(),
                Alternate = element.GetAlternate()
            };

            if(context.Updater != null && context.Updater.TryUpdateModel(viewModel, context.Prefix, null, null)) {
                element.SetName(viewModel.Name?.Trim());
                element.SetAlternate(viewModel.Alternate?.Trim());
            }

            var editorShape = context.ShapeFactory.EditorTemplate(TemplateName: "Elements/NamedElement", Model: viewModel, Prefix: context.Prefix);
            editorShape.Metadata.Position = "Meta:5";

            return Editor(context, editorShape);
        }

        protected override void OnDisplaying(Element element, ElementDisplayingContext context) {
            var name = element.GetName();
            var typeName = element.GetType().Name;
            var alternate = element.GetAlternate();
            var alternates = (IList<string>)context.ElementShape.Metadata.Alternates;

            if (!String.IsNullOrWhiteSpace(name)) {
                alternates.Add($"Elements__Named__{name}");
                alternates.Add($"Elements_{typeName}__Named__{name}");
            }

            if (!String.IsNullOrWhiteSpace(alternate)) {
                alternates.Add($"Elements__Alternate__{alternate}");
                alternates.Add($"Elements_{typeName}__Alternate__{alternate}");
            }
        }
    }
}