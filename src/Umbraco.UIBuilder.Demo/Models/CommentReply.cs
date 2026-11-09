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

        /// <summary>
        /// The author who replied, picked with the Author Picker.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// The author's name, copied from the author on save (see <c>SetReplyAuthorNameHandler</c>).
        /// </summary>
        public string AuthorName { get; set; }

        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public string Body { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
