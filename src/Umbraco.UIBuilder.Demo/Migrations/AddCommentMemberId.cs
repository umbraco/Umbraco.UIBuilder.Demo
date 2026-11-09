using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.UIBuilder.Demo.Models;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    /// <summary>
    /// Adds the MemberId column to comments. Existing comments get 0, meaning a guest.
    /// </summary>
    public class AddCommentMemberId : AsyncMigrationBase
    {
        public AddCommentMemberId(IMigrationContext context)
            : base(context)
        { }

        protected override Task MigrateAsync()
        {
            if (!ColumnExists("Comment", nameof(Comment.MemberId)))
            {
                AddColumn<Comment>(nameof(Comment.MemberId));
            }

            return Task.CompletedTask;
        }
    }
}
