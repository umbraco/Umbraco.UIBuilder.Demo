using System;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.UIBuilder.Demo.Models
{
    [TableName(TableName)]
    [PrimaryKey("Id")]
    public class CommentReply
    {
        public const string TableName = "CommentReply";

        [PrimaryKeyColumn]
        public int Id { get; set; }

        public int CommentId { get; set; }

        public string Name { get; set; }

        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public string Body { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
