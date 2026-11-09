using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.UIBuilder.Demo.Models
{
    [TableName(TableName)]
    [PrimaryKey("Id")]
    public class Topic
    {
        public const string TableName = "Topic";

        [PrimaryKeyColumn]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
