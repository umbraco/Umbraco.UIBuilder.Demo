using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;

namespace Umbraco.UIBuilder.Demo.Migrations
{
    /// <summary>
    /// Adds the "Email Address" data type used by the comment and author email fields.
    /// </summary>
    public class AddEmailAddressDataType : AsyncMigrationBase
    {
        public const string DataTypeName = "Email Address";

        private readonly IDataTypeService _dataTypeService;
        private readonly PropertyEditorCollection _propertyEditors;
        private readonly IConfigurationEditorJsonSerializer _serializer;

        public AddEmailAddressDataType(
            IMigrationContext context,
            IDataTypeService dataTypeService,
            PropertyEditorCollection propertyEditors,
            IConfigurationEditorJsonSerializer serializer)
            : base(context)
        {
            _dataTypeService = dataTypeService;
            _propertyEditors = propertyEditors;
            _serializer = serializer;
        }

        protected override async Task MigrateAsync()
        {
            if (await _dataTypeService.GetAsync(DataTypeName) is not null)
            {
                return;
            }

            if (!_propertyEditors.TryGet(Constants.PropertyEditors.Aliases.EmailAddress, out IDataEditor? editor))
            {
                throw new InvalidOperationException($"Property editor '{Constants.PropertyEditors.Aliases.EmailAddress}' was not found.");
            }

            var dataType = new DataType(editor, _serializer)
            {
                Name = DataTypeName,
                EditorUiAlias = "Umb.PropertyEditorUi.EmailAddress",
                DatabaseType = ValueStorageType.Nvarchar,
                // Data types made in code don't get the editor's default settings, so set the input type here
                ConfigurationData = new Dictionary<string, object> { ["inputType"] = "email" },
            };

            await _dataTypeService.CreateAsync(dataType, Constants.Security.SuperUserKey);
        }
    }
}
