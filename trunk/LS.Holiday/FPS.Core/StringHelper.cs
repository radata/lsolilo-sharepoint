using System;
using System.Text;
using System.Text.RegularExpressions;

namespace FPS.Core
{
    public static class StringHelper
    {
        #region Fields

        /// <summary>
        /// Represent a string containing a HTML line break.
        /// </summary>
        public static readonly string HtmlLineBreak = "<br />";

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether this string is null or empty.
        /// </summary>
        /// <param name="text">The string to be checked.</param>
        /// <returns>
        /// Is <c>true</c> if string is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Removes the HTML tags.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Plain text.</returns>
        public static string RemoveHtmlTags(this string text)
        {
            return Regex.Replace(text, @"<[^>]*>", string.Empty);
        }

        /// <summary>
        /// Determines whether value comes form lookup field.
        /// </summary>
        /// <param name="fieldValue">The field value to be checked.</param>
        /// <returns>
        ///   <c>true</c> if value comes form lookup field; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLookupField(this string fieldValue)
        {
            return fieldValue.GetLookupFieldId() > 0;
        }

        /// <summary>
        /// Gets the lookup field id.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <returns>The lookup field id.</returns>
        public static int GetLookupFieldId(this string fieldValue)
        {
            var lookupId = -1;

            if (fieldValue.IsNullOrEmpty())
                return lookupId;

            int index = fieldValue.IndexOf(SPConstants.LookupFieldSplitter, StringComparison.Ordinal);
            if (index > 0)
                int.TryParse(fieldValue.Substring(0, index), out lookupId);
            else
                int.TryParse(fieldValue, out lookupId);

            return lookupId;
        }

        /// <summary>
        /// Gets the lookup field display name.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <returns>
        /// The display name.
        /// </returns>
        public static string GetLookupFieldDisplayName(this string fieldValue)
        {
            int index = fieldValue.IndexOf(SPConstants.LookupFieldSplitter, StringComparison.Ordinal);
            if (index > 0)
                return fieldValue.Substring(index + SPConstants.LookupFieldSplitter.Length);

            return null;
        }

        /// <summary>
        /// Removes the multiple spaces.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>String with single spaces.</returns>
        public static string RemoveMultipleSpaces(this string input)
        {
            return new Regex(@"[ ]{2,}", RegexOptions.None).Replace(input, @" ").Trim();
        }

        /// <summary>
        /// Combines the URLs.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="urlParts">The URL parts.</param>
        /// <returns>Combined URL.</returns>
        public static string CombineUrls(this string baseUrl, params string[] urlParts)
        {
            var slashReplacementRegex = @"(?<=[^:])/{2,}";
            var result = new StringBuilder(baseUrl);

            // concatenate url parts
            foreach (string urlPart in urlParts)
                result.AppendFormat("/{0}", urlPart);

            // eliminate multiple slashes with the exception of the protocol part, for example: http://
            return new Regex(slashReplacementRegex, RegexOptions.None).Replace(result.ToString(), @"/");
        }

        /// <summary>
        /// Appends the HTML line break.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A new instance with appended html line break.</returns>
        public static string AppendHtmlLineBreak(this string text)
        {
            return text + HtmlLineBreak;
        }

        #endregion
    }
}
