using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.ConnectedContent.Migrations
{
    [OrchardFeature("IDeliverable.Bits.ConnectedContent")]
    public class ConnectedContentMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("ContentConnectionRecord", table => table
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("ContentId")
                .Column<int>("ConnectedContentId")
                .Column<string>("GroupName"));

            ContentDefinitionManager.AlterPartDefinition("ConnectedByPart", part => part
                .Attachable()
                .WithDescription("Displays all content items referencing this content item via their ContentPickerFields"));

            return 1;
        }
    }
}