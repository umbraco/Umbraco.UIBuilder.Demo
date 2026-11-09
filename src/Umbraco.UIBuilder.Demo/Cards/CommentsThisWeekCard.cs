using System;
using Umbraco.UIBuilder.Configuration.Cards;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Persistence;

namespace Umbraco.UIBuilder.Demo.Cards
{
    /// <summary>
    /// A custom card counting the comments posted in the last 7 days.
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

        public override object GetValue(object parentId = null)
        {
            var weekAgo = DateTime.Now.AddDays(-7);
            var result = _repositoryFactory.GetRepository<Comment, int>("comments").GetCount(x => x.DateCreated >= weekAgo);

            return result.Success ? result.Model : 0;
        }
    }
}
