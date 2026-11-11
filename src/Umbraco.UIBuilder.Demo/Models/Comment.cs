using System;
using System.ComponentModel;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Umbraco.UIBuilder.Core.Converters;

namespace Umbraco.UIBuilder.Demo.Models
{
    [TableName("Comment")]
    [PrimaryKey("Id")]
    public class Comment
    {
        [PrimaryKeyColumn]
        public int Id { get; set; }
        public string NodeUdi { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public CommentStatus Status { get; set; }

        /// <summary>
        /// The member who posted the comment, or 0 for a guest. Lets the Members section show a member's comments.
        /// </summary>
        [TypeConverter(typeof(EntityIdentifierToIntTypeConverter))]
        [Constraint(Default = "0")]
        public int MemberId { get; set; }
    }

    public enum CommentStatus
    {
        Pending,
        Approved,
        Rejected
    }
}