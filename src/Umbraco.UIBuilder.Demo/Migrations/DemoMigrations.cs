using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    /// <summary>
    /// Creates the database tables behind the demo's UI Builder collections.
    /// </summary>
    public class DemoMigrations : INotificationAsyncHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly ICoreScopeProvider _coreScopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IRuntimeState _runtimeState;

        public DemoMigrations(
            IMigrationPlanExecutor migrationPlanExecutor,
            ICoreScopeProvider coreScopeProvider,
            IKeyValueService keyValueService,
            IRuntimeState runtimeState)
        {
            _migrationPlanExecutor = migrationPlanExecutor;
            _coreScopeProvider = coreScopeProvider;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }

        public async Task HandleAsync(UmbracoApplicationStartingNotification notification, CancellationToken cancellationToken)
        {
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            var migrationPlan = new MigrationPlan("UI Builder Demo");

            migrationPlan.From(string.Empty)
                .To<AddAuthorTable>("author-table")
                .To<AddCommentReplyTable>("comment-reply-table")
                .To<AddTopicTables>("topic-tables")
                .To<AddModerationLogTable>("moderation-log-table")
                .To<AddAuthorProfileFields>("author-profile-fields")
                .To<AddCommentMemberId>("comment-member-id");

            var upgrader = new Upgrader(migrationPlan);
            await upgrader.ExecuteAsync(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
        }
    }
}
