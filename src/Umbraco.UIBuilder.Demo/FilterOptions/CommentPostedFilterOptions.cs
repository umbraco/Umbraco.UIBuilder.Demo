using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.UIBuilder.Configuration.Builders.FilterOptions;
using Umbraco.UIBuilder.Configuration.Filters;

namespace Umbraco.UIBuilder.Demo.FilterOptions
{
    /// <summary>
    /// Options for the comments "Posted" filter. Overrides the async method, as a builder that loads its options
    /// from a database or API would.
    /// </summary>
    public class CommentPostedFilterOptions : FilterOptionsBuilder<DateTime>
    {
        public override Task<IEnumerable<FilterOption<DateTime>>> GetOptionsAsync(CancellationToken cancellationToken = default)
        {
            var weekAgo = DateTime.Now.AddDays(-7);
            var monthAgo = DateTime.Now.AddDays(-30);

            IEnumerable<FilterOption<DateTime>> options =
            [
                new FilterOption<DateTime>("week", "Last 7 days", date => date >= weekAgo),
                new FilterOption<DateTime>("month", "Last 30 days", date => date >= monthAgo),
                new FilterOption<DateTime>("older", "Older than 30 days", date => date < monthAgo),
            ];

            return Task.FromResult(options);
        }
    }
}
