using System;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

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
    }

    public enum CommentStatus
    {
        Pending,
        Approved,
        Rejected
    }
}