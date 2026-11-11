using System;
using System.Collections.Generic;
using Umbraco.UIBuilder.Configuration.Builders.FilterOptions;
using Umbraco.UIBuilder.Configuration.Filters;

namespace Umbraco.UIBuilder.Demo.FilterOptions
{
    /// <summary>
    /// Options for the comments "Posted" filter, built in code by a filter options builder.
    /// </summary>
    public class CommentPostedFilterOptions : FilterOptionsBuilder<DateTime>
    {
        public override IEnumerable<FilterOption<DateTime>> GetOptions()
        {
            var weekAgo = DateTime.Now.AddDays(-7);
            var monthAgo = DateTime.Now.AddDays(-30);

            return
            [
                new FilterOption<DateTime>("week", "Last 7 days", date => date >= weekAgo),
                new FilterOption<DateTime>("month", "Last 30 days", date => date >= monthAgo),
                new FilterOption<DateTime>("older", "Older than 30 days", date => date < monthAgo),
            ];
        }
    }
}
