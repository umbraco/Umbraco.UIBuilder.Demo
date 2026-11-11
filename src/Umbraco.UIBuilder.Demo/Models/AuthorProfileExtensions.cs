using System;
using System.Linq;
using System.Text.Json;

namespace Umbraco.UIBuilder.Demo.Models
{
    /// <summary>
    /// Reads the author profile fields, which the pickers and dropdowns store as JSON, for use in templates.
    /// </summary>
    public static class AuthorProfileExtensions
    {
        /// <summary>The key of the photo's media item, if one is picked.</summary>
        public static Guid? GetPhotoMediaKey(this Author author)
            => FirstItem(author.Photo) is JsonElement item && item.TryGetProperty("mediaKey", out var key) && key.TryGetGuid(out var guid)
                ? guid
                : null;

        /// <summary>The website link's name and URL, if one is set.</summary>
        public static (string Name, string Url)? GetWebsite(this Author author)
            => FirstItem(author.Website) is JsonElement item && item.TryGetProperty("url", out var url)
                ? (item.TryGetProperty("name", out var name) ? name.GetString() : url.GetString(), url.GetString())
                : null;

        /// <summary>The picked role, if one is set.</summary>
        public static string GetRole(this Author author)
            => FirstItem(author.Role) is JsonElement item && item.ValueKind == JsonValueKind.String ? item.GetString() : null;

        private static JsonElement? FirstItem(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            using var document = JsonDocument.Parse(json);
            return document.RootElement.ValueKind == JsonValueKind.Array
                ? document.RootElement.EnumerateArray().Select(x => (JsonElement?)x.Clone()).FirstOrDefault()
                : document.RootElement.Clone();
        }
    }
}
