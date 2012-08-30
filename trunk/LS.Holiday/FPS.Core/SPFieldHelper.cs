using System;
using System.Collections.Generic;
using System.Linq;
using FPS.Core.Enums;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class SPFieldHelper
    {
        #region Fields

        private static readonly string CustomizationAttribute = "Customization";
        private static readonly char PropertySplitter = ';';
        private static readonly char TypeSplitter = ':';
        private static readonly char ValueSplitter = ',';

        #endregion

        #region Methods

        /// <summary>
        /// Gets list of values from Customization attribute.
        /// </summary>
        /// <typeparam name="T">List item type.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="type">The type of property.</param>
        /// <returns>The list of properties.</returns>
        public static List<T> GetCustomizationValues<T>(this SPField field, CustomPropertyType type)
        {
            var result = new List<T>();
            var atributeValue = field.GetProperty(CustomizationAttribute);

            if (atributeValue == null)
                return null;

            var typeName = type.ToString();
            var properties = atributeValue.Split(PropertySplitter).FirstOrDefault(p => p.StartsWith(typeName));

            if (properties == null)
                return null;

            // Getting values
            var values = properties.Replace(typeName + TypeSplitter, string.Empty).Split(ValueSplitter).ToList();
            if (values.Count == 0)
                return result;

            // Conversion
            values.ForEach(value => result.Add((T)Convert.ChangeType(value, typeof(T))));

            return result;
        }

        /// <summary>
        /// Gets first or default value from Customization attribute.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="type">The type of property.</param>
        /// <returns>First or default value.</returns>
        public static T GetCustomizationValue<T>(this SPField field, CustomPropertyType type)
        {
            var customization = field.GetCustomizationValues<T>(type);
            return customization != null ? customization.FirstOrDefault() : default(T);
        }

        #endregion
    }
}
