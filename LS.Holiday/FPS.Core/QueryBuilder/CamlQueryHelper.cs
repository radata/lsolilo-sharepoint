using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core;
using FPS.Core.QueryBuilder.Enums;
using FPS.Core.QueryBuilder.Interfaces;
using FPS.Core.QueryBuilder.QuerySchemaElements.Base;
using FPS.Evaluation.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace FPS.Core.QueryBuilder
{
    /// <summary>
    /// Caml Query Helper class.
    /// </summary>
    public static class CamlQueryHelper
    {
        #region Fields

        private static readonly string[] WhereSplitingArray = new string[] { "<Where>", "</Where>" };
        private static readonly string[] QuerySplitingArray = new string[] { "<Query>", "</Query>" };

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether field can be used in query.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// <c>True</c> if is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidFieldForQuery(SPFieldType fieldType, string fieldName)
        {
            return
                !(fieldType == SPFieldType.Computed && fieldName == SPBuiltInFieldNames.Edit) &&
                !(fieldType == SPFieldType.Lookup && fieldName == SPBuiltInFieldNames.FolderChildCount);
        }

        /// <summary>
        /// Tries the parse value.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns>True if parse successful, otherwise False.</returns>
        public static bool TryParseValue(SPFieldType fieldType, ref string searchText)
        {
            bool isParsedSuccessfully = false;

            switch (fieldType)
            {
                case SPFieldType.Boolean:
                    BoolenValues boolenValue;
                    isParsedSuccessfully = EnumHelper.TryParse(searchText, out boolenValue);

                    if (isParsedSuccessfully)
                        searchText = ((int)boolenValue).ToString();

                    return isParsedSuccessfully;

                case SPFieldType.WorkflowEventType:
                case SPFieldType.Counter:
                case SPFieldType.Integer:
                    int intValue;
                    return int.TryParse(searchText, out intValue);

                case SPFieldType.Number:
                case SPFieldType.Currency:
                    double doubleValue;
                    isParsedSuccessfully = double.TryParse(searchText, out doubleValue);

                    if (isParsedSuccessfully)
                        searchText = doubleValue.ToString().Replace(',', '.');

                    return isParsedSuccessfully;

                case SPFieldType.DateTime:
                    DateTime dateValue;
                    isParsedSuccessfully = DateTime.TryParse(searchText, out dateValue);

                    if (isParsedSuccessfully)
                        searchText = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dateValue);

                    return isParsedSuccessfully;

                case SPFieldType.ContentTypeId:
                case SPFieldType.AllDayEvent:
                case SPFieldType.Attachments:
                case SPFieldType.CrossProjectLink:
                case SPFieldType.PageSeparator:
                case SPFieldType.Error:
                case SPFieldType.File:
                case SPFieldType.GridChoice:
                case SPFieldType.Invalid:
                case SPFieldType.ThreadIndex:
                case SPFieldType.Threading:
                case SPFieldType.MaxItems:
                case SPFieldType.Recurrence:
                    return false;

                case SPFieldType.ModStat:
                case SPFieldType.MultiChoice:
                case SPFieldType.Calculated:
                case SPFieldType.Choice:
                case SPFieldType.WorkflowStatus:
                case SPFieldType.Guid:
                case SPFieldType.URL:
                case SPFieldType.Text:
                case SPFieldType.Note:
                case SPFieldType.Lookup:
                case SPFieldType.User:
                case SPFieldType.Computed:
                    return true;
            }

            return true;
        }

        /// <summary>
        /// Merges the query condition.
        /// </summary>
        /// <param name="querySchemaElement">The query schema element.</param>
        /// <param name="mainQuery">The main query.</param>
        /// <param name="queryItems">The query items.</param>
        /// <returns>Returns merged query.</returns>
        public static CamlQuery MergeQueryCondition(CamlQuerySchemaElements querySchemaElement, CamlQuery mainQuery, params CamlQuery[] queryItems)
        {
            var queryItemList = queryItems.ToList();

            if (mainQuery == null || !queryItemList.Any())
                return null;

            foreach (CamlQuery camlQuery in queryItems)
            {
                mainQuery.ValidFields.AddRange(camlQuery.ValidFields);
                mainQuery.InvalidFields.AddRange(camlQuery.InvalidFields);

                var newQueryCondition = string.Empty;
                var itemQueryConditions = camlQuery.Query.Split(WhereSplitingArray, StringSplitOptions.None);

                // If there is no wehre conditon or condition is empty merge not requierd
                if (itemQueryConditions.Length < WhereSplitingArray.Length || itemQueryConditions[1].IsNullOrEmpty())
                    continue;

                // If there is where condition in main query
                if (mainQuery.Query.Contains(WhereSplitingArray[0]))
                {
                    var mainQueryCondition = mainQuery.Query.Split(WhereSplitingArray, StringSplitOptions.None)[1];

                    // If where query in main query is empty insert item query only
                    if (mainQueryCondition.IsNullOrEmpty())
                        mainQuery.Query = mainQuery.Query.Replace(WhereSplitingArray[0], WhereSplitingArray[0] + itemQueryConditions[1]);
                    else
                    {
                        newQueryCondition = CreateQueryCondition(querySchemaElement, mainQueryCondition, itemQueryConditions[1]);
                        mainQuery.Query = mainQuery.Query.Replace(mainQueryCondition, newQueryCondition);
                    }
                }
                else
                {
                    mainQuery.Where = CreateQueryCondition(CamlQuerySchemaElements.Where, itemQueryConditions[1]);
                }
            }

            return mainQuery;
        }

        /// <summary>
        /// Creates the query condition.
        /// </summary>
        /// <param name="querySchemaElement">The query schema elements.</param>
        /// <param name="queryItems">The query items.</param>
        /// <returns>Returns query condition.</returns>
        public static string CreateQueryCondition(CamlQuerySchemaElements querySchemaElement, params string[] queryItems)
        {
            var elementSchema = EnumHelper.GetStringValue(querySchemaElement, typeof(CamlQuerySchemaElementsAttribute));
            return elementSchema.Replace(CamQueryVariables.Value, string.Join(string.Empty, queryItems));
        }

        /// <summary>
        /// Gets the attributes string.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>Returns attributes as string.</returns>
        public static string GetAttributes(Dictionary<CamlQuerySchemaAttributes, object> attributes)
        {
            if (attributes == null)
                return string.Empty;

            var result = attributes
                .Select(attribute => EnumHelper.GetStringValue(attribute.Key, typeof(CamlQuerySchemaElementsAttribute)).Replace("%Value%", attribute.Value == null ? string.Empty : attribute.Value.ToString()))
                .ToArray();

            return string.Join(" ", result);
        }

        #endregion
    }
}
