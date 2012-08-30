using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class SPItemEventPropertiesHelper
    {
        #region Methods

        /// <summary>
        /// Determines whether value has changed.
        /// </summary>
        /// <param name="itemEventProperties">The item event properties.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// <c>True</c> if value has changed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValueChanged(this SPItemEventProperties itemEventProperties, string fieldName)
        {
            if (fieldName.IsNullOrEmpty())
                return false;

            if (!itemEventProperties.ListItem.Fields.ContainsField(fieldName))
                return false;

            var isValueChanged = true;
            var oldValue = itemEventProperties.ListItem[fieldName];
            var newValue = itemEventProperties.AfterProperties[fieldName];

            // Value hasn't been changed.
            var properties = typeof(SPItemEventDataCollection).GetProperty("Properties", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(itemEventProperties.AfterProperties, null) as Hashtable;
            if (!properties.ContainsKey(fieldName))
                return false;

            if (newValue as string == string.Empty)
                newValue = null;

            if (oldValue != null && newValue != null)
            {
                var valueType = oldValue.GetType();

                switch (itemEventProperties.ListItem.Fields.GetField(fieldName).Type)
                {
                    case SPFieldType.DateTime:
                        isValueChanged = ((DateTime)Convert.ChangeType(newValue, valueType)).ToLocalTime() != ((DateTime)Convert.ChangeType(oldValue, valueType)).ToLocalTime();
                        break;
                    case SPFieldType.Lookup:
                        isValueChanged = !new SPFieldLookupValueCollection(newValue as string).Select(item => item.LookupId).ToArray().SequenceEqual(new SPFieldLookupValueCollection(oldValue as string).Select(item => item.LookupId).ToArray());
                        break;
                    default:
                        isValueChanged = Convert.ChangeType(newValue, valueType, CultureInfo.InvariantCulture).ToString() != Convert.ChangeType(oldValue, valueType).ToString();
                        break;
                }
            }
            else
            {
                isValueChanged = newValue != oldValue;
            }

            return isValueChanged;
        }

        #endregion
    }
}
