using System.Linq;
using System.Xml.XPath;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Recipes.Services;

namespace IDeliverable.Bits.Recipes.Builders
{
    [OrchardFeature("IDeliverable.Bits.Recipes")]
    public class RemovePublishedUtcStep : RecipeBuilderStep
    {
        public override string Name => "RemovePublishedUtc";

        public override LocalizedString DisplayName => T("Remove Published Utc");

        public override LocalizedString Description => T("Removes all PublishedUtc attributes of the CommonPart element.");

        public override int Priority => -900;
        public override int Position => 900;

        public override void Build(BuildContext context)
        {
            var rootElement = context.RecipeDocument.Element("Orchard");
            var commonPartElements = rootElement.XPathSelectElements("//CommonPart").ToList();
            var publishedUtcAttributes = commonPartElements.Select(x => x.Attribute("PublishedUtc")).Where(x => x != null);

            foreach (var publishedUtcAttribute in publishedUtcAttributes)
            {
                publishedUtcAttribute.Remove();
            }
        }
    }
}