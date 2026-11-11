using System;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.UIBuilder.Demo.Models
{
    /// <summary>
    /// A record of a comment's status changing, written by <c>LogCommentStatusChangeHandler</c>.
    /// </summary>
    [TableName(TableName)]
    [PrimaryKey("Id")]
    public class ModerationLogEntry
    {
        public const string TableName = "ModerationLog";

        [PrimaryKeyColumn]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string CommentName { get; set; }
        public CommentStatus FromStatus { get; set; }
        public CommentStatus ToStatus { get; set; }
        public string ChangedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
