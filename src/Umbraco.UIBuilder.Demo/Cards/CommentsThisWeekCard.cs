using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.UIBuilder.Configuration.Cards;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Persistence;

namespace Umbraco.UIBuilder.Demo.Cards
{
    /// <summary>
    /// A custom card counting the comments posted in the last 7 days, worked out asynchronously.
    /// </summary>
    public class CommentsThisWeekCard : Card
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public CommentsThisWeekCard(IRepositoryFactory repositoryFactory)
            : base("commentsThisWeek", "Posted This Week")
        {
            _repositoryFactory = repositoryFactory;
            Icon = "icon-calendar";
            Color = "green";
        }

        public override async Task<object> GetValueAsync(object parentId = null, CancellationToken cancellationToken = default)
        {
            var weekAgo = DateTime.Now.AddDays(-7);
            var result = await _repositoryFactory.GetRepository<Comment, int>("comments")
                .GetCountAsync(x => x.DateCreated >= weekAgo, cancellationToken: cancellationToken);

            return result.Success ? result.Model : 0;
        }
    }
}
