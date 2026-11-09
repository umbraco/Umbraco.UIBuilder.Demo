using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Events;

namespace Umbraco.UIBuilder.Demo.Events
{
    /// <summary>
    /// Deletes a comment's replies when the comment is deleted, so no orphaned replies are left behind.
    /// </summary>
    public class DeleteCommentRepliesHandler : INotificationHandler<EntityDeletedNotification>
    {
        private readonly IScopeProvider _scopeProvider;

        public DeleteCommentRepliesHandler(IScopeProvider scopeProvider)
            => _scopeProvider = scopeProvider;

        public void Handle(EntityDeletedNotification notification)
        {
            if (notification.Entity is not Comment comment)
            {
                return;
            }

            using IScope scope = _scopeProvider.CreateScope();
            scope.Database.Delete<CommentReply>("WHERE CommentId = @0", comment.Id);
            scope.Complete();
        }
    }
}
