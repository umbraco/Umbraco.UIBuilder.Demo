using System;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Events;

namespace Umbraco.UIBuilder.Demo.Events
{
    /// <summary>
    /// Writes a moderation log entry whenever an existing comment's status changes, whether from the editor or
    /// the Change Status action.
    /// </summary>
    public class LogCommentStatusChangeHandler : INotificationHandler<EntitySavedNotification>
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

        public LogCommentStatusChangeHandler(IScopeProvider scopeProvider, IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
        {
            _scopeProvider = scopeProvider;
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        }

        public void Handle(EntitySavedNotification notification)
        {
            // New comments have no "before", so only changes to existing comments are logged
            if (notification.Entity.Before is not Comment before
                || notification.Entity.After is not Comment after
                || before.Status == after.Status)
            {
                return;
            }

            using IScope scope = _scopeProvider.CreateScope();
            scope.Database.Insert(new ModerationLogEntry
            {
                CommentId = after.Id,
                CommentName = after.Name,
                FromStatus = before.Status,
                ToStatus = after.Status,
                ChangedBy = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser?.Name ?? "Unknown",
                DateCreated = DateTime.Now,
            });
            scope.Complete();
        }
    }
}
