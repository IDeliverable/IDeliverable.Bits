using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Navigation.Migrations
{
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition("OmniNavigationMenuItem",
               cfg => cfg
                   .WithPart("MenuPart")
                   .WithPart("CommonPart")
                   .WithPart("IdentityPart")
                   .DisplayedAs("Omni Link")
                   .WithSetting("Description", "Injects menu items from all menus.")
                   .WithSetting("Stereotype", "MenuItem")
               );

            ContentDefinitionManager.AlterTypeDefinition("ActionLink",
                cgf => cgf
                    .WithPart("ActionLinkPart")
                    .WithPart("MenuPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .DisplayedAs("Action Link")
                    .WithSetting("Description", "Represents a link to a configured controller, action and route values.")
                    .WithSetting("Stereotype", "MenuItem")
                );

            return 2;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("ActionLink",
                cgf => cgf
                    .WithPart("ActionLinkPart")
                    .WithPart("MenuPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .DisplayedAs("Action Link")
                    .WithSetting("Description", "Represents a link to a configured controller, action and route values.")
                    .WithSetting("Stereotype", "MenuItem")
                );

            return 2;
        }
    }
}