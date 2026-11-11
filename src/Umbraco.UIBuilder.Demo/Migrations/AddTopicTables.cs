using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.UIBuilder.Demo.Models;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    public class AddTopicTables : AsyncMigrationBase
    {
        public AddTopicTables(IMigrationContext context)
            : base(context)
        { }

        protected override Task MigrateAsync()
        {
            if (TableExists(Topic.TableName))
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", Topic.TableName);
                return Task.CompletedTask;
            }

            Create.Table<Topic>().Do();
            Create.Table<AuthorTopic>().Do();

            // Sample topics (ids 1-5) and which authors write about them
            foreach (var name in new[] { "Back office", "Content modelling", "Editor experience", "Integrations", "Performance" })
            {
                Database.Insert(new Topic { Name = name });
            }

            foreach (var (authorId, topicId) in new[] { (1, 1), (1, 4), (1, 5), (2, 2), (2, 3) })
            {
                Database.Insert(new AuthorTopic { AuthorId = authorId, TopicId = topicId });
            }

            return Task.CompletedTask;
        }
    }
}
