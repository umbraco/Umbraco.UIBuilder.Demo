using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.UIBuilder.Demo.Models;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    public class AddCommentReplyTable : AsyncMigrationBase
    {
        public AddCommentReplyTable(IMigrationContext context)
            : base(context)
        { }

        protected override Task MigrateAsync()
        {
            if (TableExists(CommentReply.TableName))
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", CommentReply.TableName);
                return Task.CompletedTask;
            }

            Create.Table<CommentReply>().Do();

            // Sample replies to two of the approved demo comments
            Database.Insert(new CommentReply
            {
                CommentId = 1,
                Name = "Hannah Brooks",
                Body = "Thanks Matt, glad it helped!",
                DateCreated = new DateTime(2025, 3, 4)
            });
            Database.Insert(new CommentReply
            {
                CommentId = 14,
                Name = "Sam Okafor",
                Body = "Thanks James, glad you enjoyed it!",
                DateCreated = new DateTime(2025, 3, 5)
            });

            return Task.CompletedTask;
        }
    }
}
