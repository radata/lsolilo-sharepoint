using System;
using System.Linq;
using FPS.Core.Attributes;

namespace FPS.Core
{
    public static class EnumHelper
    {
        /// <summary>
        /// Tries to parse string to enum.
        /// </summary>
        /// <typeparam name="T">Output enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if parse successful, otherwise False.</returns>
        public static bool TryParse<T>(string enumValue, out T result)
        {
            result = default(T);

            if (!enumValue.IsNullOrEmpty() && (Enum.IsDefined(typeof(T), enumValue) || Enum.GetNames(typeof(T)).Any(name => string.Compare(name, enumValue, true) == 0)))
            {
                result = (T)Enum.Parse(typeof(T), enumValue, true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to parse string to enum.
        /// </summary>
        /// <typeparam name="T">Output enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Returns parsed enum value or default one.</returns>
        public static T SafeParse<T>(string enumValue, T defaultValue)
        {
            T result = defaultValue;

            if (!enumValue.IsNullOrEmpty() && Enum.IsDefined(typeof(T), enumValue))
                result = (T)Enum.Parse(typeof(T), enumValue);

            return result;
        }

        /// <summary>
        /// Tries to convert one enum value to another.
        /// </summary>
        /// <typeparam name="T">Output enum type.</typeparam>
        /// <param name="sourceEnum">The source enum.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if parse successful, otherwise False.</returns>
        public static bool TryConvert<T>(this Enum sourceEnum, out T result)
        {
            return EnumHelper.TryParse(sourceEnum.ToString(), out result);
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns>
        /// Attribute string value of enum if assigned; null otherwise.
        /// </returns>
        public static string GetStringValue(this Enum value, Type attributeType)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(attributeType, false) as StringValueAttributeBase[];

            return attributes.Length > 0 ? attributes[0].StringValue : null;
        }
    }
}
