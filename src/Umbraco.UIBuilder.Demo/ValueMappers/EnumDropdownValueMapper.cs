using System;
using Umbraco.UIBuilder.Mapping;
using Newtonsoft.Json;

namespace Umbraco.UIBuilder.Demo.ValueMappers
{
    public class EnumDropdownValueMapper<TEnumType> : ValueMapper
        where TEnumType : struct, Enum
    {
        public override object ModelToEditor(object input)
        {
            var vals = Enum.GetValues<TEnumType>();
            var val = input != null && !string.IsNullOrWhiteSpace(input.ToString())
                ? (TEnumType)Enum.Parse(typeof(TEnumType), input.ToString(), true)
                : vals[0];

            return JsonConvert.SerializeObject(new[] {val.ToString()});
        }

        public override object EditorToModel(object input)
        {
            var rawVal = input != null && !string.IsNullOrWhiteSpace(input.ToString())
                ? JsonConvert.DeserializeObject<string[]>(input.ToString())
                : Array.Empty<string>();
            
            var vals = Enum.GetValues<TEnumType>();
            var val = rawVal != null && rawVal.Length > 0
                ? (TEnumType)(TEnumType)Enum.Parse(typeof(TEnumType), rawVal[0], true)
                : vals[0];

            return val.ToString();
        }
    }
}
