using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.UIBuilder.Configuration.Builders.DataViews;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Models;

namespace Umbraco.UIBuilder.Demo.DataViews
{
    public class CommentStatusDataViewsBuilder : DataViewsBuilder<Comment>
    {
        public override IEnumerable<DataViewSummary> GetDataViews()
        {
            yield return new DataViewSummary
            {
                Name = "All",
                Alias = "all",
                Group = "Status"
            };
            
            foreach (var val in Enum.GetValues<CommentStatus>())
            {
                yield return new DataViewSummary
                {
                    Name = val.ToString(),
                    Alias = val.ToString().ToLower(),
                    Group = "Status"
                };
            }
        }

        public override Expression<Func<Comment, bool>> GetDataViewWhereClause(string dataViewAlias)
        {
            if (dataViewAlias == "all")
                return null;

            var commentStatus = Enum.Parse<CommentStatus>(dataViewAlias, true);
            
            return c => c.Status == commentStatus;
        }
    }
}
