using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Relationships.Migrations
{
    [OrchardFeature("IDeliverable.Bits.Relationships")]
    public class RelationshipsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("ContentRelationRecord", table => table
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("ContentItemId")
                .Column<int>("RelatedContentItemId")
                .Column<string>("Discriminator"));

            ContentDefinitionManager.AlterPartDefinition("RelatedByPart", part => part
                .Attachable()
                .WithDescription("Displays all content items referencing this content item via their ContentPickerFields"));

            return 1;
        }
    }
}