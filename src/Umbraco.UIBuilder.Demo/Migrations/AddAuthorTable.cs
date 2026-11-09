using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.UIBuilder.Demo.Models;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    public class AddAuthorTable : AsyncMigrationBase
    {
        public AddAuthorTable(IMigrationContext context)
            : base(context)
        { }

        protected override Task MigrateAsync()
        {
            if (TableExists(Author.TableName))
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", Author.TableName);
                return Task.CompletedTask;
            }

            Create.Table<Author>().Do();

            // Sample authors for the demo
            Database.Insert(new Author
            {
                Name = "Hannah Brooks",
                JobTitle = "Lead Developer",
                Email = "hannah@example.com",
                Bio = "<p>Hannah builds back office tools for content teams and has been working with Umbraco since v7.</p>",
                DateCreated = new DateTime(2025, 1, 6)
            });
            Database.Insert(new Author
            {
                Name = "Sam Okafor",
                JobTitle = "Content Strategist",
                Email = "sam@example.com",
                Bio = "<p>Sam helps editors get the most out of their CMS and writes about content workflows.</p>",
                DateCreated = new DateTime(2025, 2, 3)
            });

            return Task.CompletedTask;
        }
    }
}
