using System;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.UIBuilder.Demo.Models
{
    [TableName(TableName)]
    [PrimaryKey("Id")]
    public class Author
    {
        public const string TableName = "Author";

        [PrimaryKeyColumn]
        public int Id { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }

        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Bio { get; set; }

        public DateTime DateCreated { get; set; }

        // Profile fields, one per editor type. Picker and dropdown values are stored as JSON.

        /// <summary>Single media picker.</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public string Photo { get; set; }

        /// <summary>Single URL picker.</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public string Website { get; set; }

        /// <summary>Single dropdown.</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public string Role { get; set; }

        /// <summary>Multiple dropdown.</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public string Languages { get; set; }

        /// <summary>Slider, stored as text such as "6".</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public string YearsWriting { get; set; }

        /// <summary>Range slider, stored as text such as "4,8".</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public string PostLength { get; set; }

        /// <summary>Integer with a validation range.</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? PostsPerMonth { get; set; }

        /// <summary>Decimal with a validation range.</summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public decimal? ReaderRating { get; set; }
    }
}
