using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.UIBuilder.Demo.Models
{
    /// <summary>
    /// Junction table linking authors to the topics they write about.
    /// </summary>
    [TableName(TableName)]
    [PrimaryKey("AuthorId,TopicId", AutoIncrement = false)]
    public class AuthorTopic
    {
        public const string TableName = "AuthorTopic";

        [PrimaryKeyColumn(AutoIncrement = false, OnColumns = "AuthorId, TopicId")]
        public int AuthorId { get; set; }
        public int TopicId { get; set; }
    }
}
