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

            return 1;
        }
    }
}