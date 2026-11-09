using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Events;

namespace Umbraco.UIBuilder.Demo.Events
{
    /// <summary>
    /// Removes the author/topic links when an author or a topic is deleted, so the junction table has no orphaned rows.
    /// </summary>
    public class DeleteAuthorTopicLinksHandler : INotificationHandler<EntityDeletedNotification>
    {
        private readonly IScopeProvider _scopeProvider;

        public DeleteAuthorTopicLinksHandler(IScopeProvider scopeProvider)
            => _scopeProvider = scopeProvider;

        public void Handle(EntityDeletedNotification notification)
        {
            (string column, int id) = notification.Entity switch
            {
                Author author => (nameof(AuthorTopic.AuthorId), author.Id),
                Topic topic => (nameof(AuthorTopic.TopicId), topic.Id),
                _ => (null, 0),
            };

            if (column == null)
            {
                return;
            }

            using IScope scope = _scopeProvider.CreateScope();
            scope.Database.Delete<AuthorTopic>($"WHERE {column} = @0", id);
            scope.Complete();
        }
    }
}
