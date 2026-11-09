using Umbraco.UIBuilder.Configuration.Actions;
using Umbraco.UIBuilder.Configuration.Builders;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Demo.ValueMappers;
using Umbraco.UIBuilder.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Umbraco.UIBuilder.Demo.Actions
{
    public class ChangeStatusAction : Configuration.Actions.Action<ChangeStatusSettings, ActionResult>
    {
        private readonly IRepositoryFactory _repoFactory;

        public override string Icon => "icon-nodes";
        public override string Alias => "changestatus";
        public override string Name => "Change Status";

        public ChangeStatusAction(IRepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;
        }

        public override void Configure(SettingsConfigBuilder<ChangeStatusSettings> settingsConfig)
        {
            settingsConfig.AddFieldset("General", fieldsetConfig => fieldsetConfig
                .AddField(s => s.Status).SetDataType("Comment Status").SetValueMapper<EnumDropdownValueMapper<CommentStatus>>()
            );
        }

        public override bool IsVisible(ActionVisibilityContext ctx)
        {
            return ctx.ActionType == ActionType.Bulk
                || ctx.ActionType == ActionType.Row;
        }

        public override async Task<ActionResult> ExecuteAsync(string collectionAlias, object[] entityIds, ChangeStatusSettings settings, CancellationToken cancellationToken = default)
        {
            try
            {
                var repo = _repoFactory.GetRepository<Comment, int>(collectionAlias);

                var ids = entityIds.Select(x => int.Parse(x?.ToString())).ToArray();
                var result = await repo.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: cancellationToken);

                if (result.Success)
                {
                    foreach (var entity in result.Model)
                    {
                        entity.Status = settings.Status;

                        await repo.SaveAsync(entity, cancellationToken: cancellationToken);
                    }
                }

                return new ActionResult(true);
            }
            catch (Exception ex)
            {
                return new ActionResult(false, new ActionNotification("Failed to update status", ex.Message));
            }
        }
    }

    public class ChangeStatusSettings
    {
        public CommentStatus Status { get; set; }
    }
}
