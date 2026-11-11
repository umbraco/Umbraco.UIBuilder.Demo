using System;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.UIBuilder.Demo.Models
{
    [TableName(TableName)]
    [PrimaryKey("Id")]
    public class Author
    {
        public const string TableName = "Author";

        [PrimaryKeyColumn]
        public int Id { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }

        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Bio { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
