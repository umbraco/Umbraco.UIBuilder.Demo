using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Events;

namespace Umbraco.UIBuilder.Demo.Events
{
    /// <summary>
    /// Copies the picked author's name onto a reply when it's saved, so lists and the website can show it
    /// without looking the author up each time.
    /// </summary>
    public class SetReplyAuthorNameHandler : INotificationHandler<EntitySavingNotification>
    {
        private readonly IScopeProvider _scopeProvider;

        public SetReplyAuthorNameHandler(IScopeProvider scopeProvider)
            => _scopeProvider = scopeProvider;

        public void Handle(EntitySavingNotification notification)
        {
            if (notification.Entity.After is not CommentReply reply)
            {
                return;
            }

            using IScope scope = _scopeProvider.CreateScope(autoComplete: true);
            reply.AuthorName = scope.Database.ExecuteScalar<string>(
                $"SELECT Name FROM {Author.TableName} WHERE Id = @0", reply.AuthorId);
        }
    }
}
