using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.UIBuilder.Demo.Models;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    public class AddModerationLogTable : AsyncMigrationBase
    {
        public AddModerationLogTable(IMigrationContext context)
            : base(context)
        { }

        protected override Task MigrateAsync()
        {
            if (TableExists(ModerationLogEntry.TableName))
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", ModerationLogEntry.TableName);
                return Task.CompletedTask;
            }

            // Columns are listed explicitly because the typed Create.Table<T>() can't map enum properties
            Create.Table(ModerationLogEntry.TableName)
                .WithColumn(nameof(ModerationLogEntry.Id)).AsInt32().NotNullable().PrimaryKey($"PK_{ModerationLogEntry.TableName}").Identity()
                .WithColumn(nameof(ModerationLogEntry.CommentId)).AsInt32().NotNullable()
                .WithColumn(nameof(ModerationLogEntry.CommentName)).AsString(255).NotNullable()
                .WithColumn(nameof(ModerationLogEntry.FromStatus)).AsInt32().NotNullable()
                .WithColumn(nameof(ModerationLogEntry.ToStatus)).AsInt32().NotNullable()
                .WithColumn(nameof(ModerationLogEntry.ChangedBy)).AsString(255).NotNullable()
                .WithColumn(nameof(ModerationLogEntry.DateCreated)).AsDateTime().NotNullable()
                .Do();

            // Sample history matching the demo comments' current statuses
            foreach (var (commentId, name, to, date) in new[]
            {
                (1, "Matt Hunter", CommentStatus.Approved, new DateTime(2025, 3, 1, 9, 30, 0)),
                (4, "Matt Linton", CommentStatus.Approved, new DateTime(2025, 3, 1, 9, 32, 0)),
                (12, "Martin Huston", CommentStatus.Rejected, new DateTime(2025, 3, 2, 14, 5, 0)),
                (14, "James Stevens", CommentStatus.Approved, new DateTime(2025, 3, 3, 11, 12, 0)),
                (17, "Matt Smith", CommentStatus.Rejected, new DateTime(2025, 3, 3, 11, 15, 0)),
            })
            {
                Database.Insert(new ModerationLogEntry
                {
                    CommentId = commentId,
                    CommentName = name,
                    FromStatus = CommentStatus.Pending,
                    ToStatus = to,
                    ChangedBy = "Admin",
                    DateCreated = date,
                });
            }

            return Task.CompletedTask;
        }
    }
}
