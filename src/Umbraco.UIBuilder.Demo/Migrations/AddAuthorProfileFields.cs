using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.UIBuilder.Demo.Models;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    /// <summary>
    /// Adds the author profile columns to existing databases and fills them in for the sample authors.
    /// </summary>
    public class AddAuthorProfileFields : AsyncMigrationBase
    {
        public AddAuthorProfileFields(IMigrationContext context)
            : base(context)
        { }

        protected override Task MigrateAsync()
        {
            // AddColumn<T> takes each column's definition from the Author model
            foreach (var column in new[]
            {
                nameof(Author.Photo), nameof(Author.Website), nameof(Author.Role), nameof(Author.Languages),
                nameof(Author.YearsWriting), nameof(Author.PostLength), nameof(Author.PostsPerMonth), nameof(Author.ReaderRating),
            })
            {
                if (!ColumnExists(Author.TableName, column))
                {
                    AddColumn<Author>(column);
                }
            }

            // Sample profile values (photos are media items, so they're set up with the media)
            Database.Execute(
                $"UPDATE {Author.TableName} SET Website = @0, Role = @1, Languages = @2, YearsWriting = @3, PostLength = @4, PostsPerMonth = @5, ReaderRating = @6 WHERE Id = 1",
                "[{\"name\":\"hannah.example.com\",\"url\":\"https://hannah.example.com\",\"target\":\"_blank\",\"type\":\"external\"}]",
                "[\"Staff writer\"]", "[\"English\",\"Danish\"]", "9", "6,10", 4, 4.7m);
            Database.Execute(
                $"UPDATE {Author.TableName} SET Website = @0, Role = @1, Languages = @2, YearsWriting = @3, PostLength = @4, PostsPerMonth = @5, ReaderRating = @6 WHERE Id = 2",
                "[{\"name\":\"sam.example.com\",\"url\":\"https://sam.example.com\",\"target\":\"_blank\",\"type\":\"external\"}]",
                "[\"Guest author\"]", "[\"English\"]", "4", "3,5", 2, 4.3m);

            return Task.CompletedTask;
        }
    }
}
