using System.Text.RegularExpressions;

namespace WearCast.Api.Common.Extensions
{
    public static partial class StringExtensions
    {
        [GeneratedRegex(@"[^a-z0-9\s-]")]
        private static partial Regex SpecialCharactersRegex();

        [GeneratedRegex(@"\s+")]
        private static partial Regex SpacesRegex();

        public static string ToUniqueSlug(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            string slug = text.ToLowerInvariant();

            slug = SpecialCharactersRegex().Replace(slug, "");
            slug = SpacesRegex().Replace(slug, "-").Trim('-');

            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 5);

            return $"{slug}-{uniqueId}";
        }
    }
}
