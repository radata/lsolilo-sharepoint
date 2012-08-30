using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core;
using FPS.Core.QueryBuilder.Enums;
using FPS.Core.QueryBuilder.Interfaces;
using Microsoft.SharePoint;

namespace FPS.Core.QueryBuilder.QuerySchemaElements.Base
{
    /// <summary>
    /// Base class for all CamlQuery Comparison Operators.
    /// </summary>
    public abstract class CamlQueryComparisonOperator : IQueryElement
    {
        #region Fields

        protected CamlQuerySchemaElements _elementType;
        protected SPList _list;
        protected SPField _field;
        protected SPFieldType _fieldType = SPFieldType.Invalid;
        protected bool _isValidCondition;
        protected bool? _isLookupId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>
        /// The type of the field.
        /// </value>
        public SPFieldType FieldType
        {
            get
            {
                if (_fieldType != SPFieldType.Invalid)
                    return _fieldType;
                else if (Field != null)
                    return Field.Type;
                else
                    return SPFieldType.Invalid;
            }

            protected set
            {
                _fieldType = value;
            }
        }

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        /// <value>
        /// The field value.
        /// </value>
        public string FieldValue { get; protected set; }

        /// <summary>
        /// Gets the field.
        /// </summary>
        public SPField Field
        {
            get
            {
                if (_list != null && _list.Fields.ContainsField(FieldName))
                    _field = _list.Fields.GetField(FieldName);

                return _field;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this field is valid list field.
        /// </summary>
        /// <value>
        /// <c>True</c> if this field is valid list field; otherwise, <c>false</c>.
        /// </value>
        public bool IsFieldValidListField
        {
            get
            {
                return _list == null || Field != null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this field is lookup id.
        /// </summary>
        /// <value>
        /// <c>True</c> if this field is lookup id; otherwise, <c>false</c>.
        /// </value>
        public bool IsLookupId
        {
            get
            {
                if (_isLookupId.HasValue)
                {
                    return _isLookupId.Value;
                }
                else if (Field != null && Field.Id == SPBuiltInFieldId.ItemChildCount)
                {
                    return false;
                }
                else if (Field is SPFieldLookup || FieldType == SPFieldType.Lookup)
                {
                    var lookupField = Field as SPFieldLookup;
                    if (lookupField != null && lookupField.CountRelated)
                        return false;

                    int fieldValue;
                    return int.TryParse(FieldValue, out fieldValue);
                }
                else
                {
                    return false;
                }
            }

            set
            {
                _isLookupId = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CamlQueryComparisonOperator"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="elementType">Type of the element.</param>
        public CamlQueryComparisonOperator(string fieldName, SPFieldType fieldType, string fieldValue, CamlQuerySchemaElements elementType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FieldValue = fieldValue;
            _elementType = elementType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CamlQueryComparisonOperator"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="elementType">Type of the element.</param>
        public CamlQueryComparisonOperator(string fieldName, string fieldValue, CamlQuerySchemaElements elementType)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
            _elementType = elementType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>Return query.</returns>
        public virtual CamlQuery GetQuery(SPList list = null)
        {
            _list = list;
            return GetQuery();
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <returns>Return query.</returns>
        protected virtual CamlQuery GetQuery()
        {
            var fieldValue = FieldValue;
            var fieldType = FieldType;
            var aditionalFieldAttributes = new Dictionary<CamlQuerySchemaAttributes, object>();
            var aditionalValueAttributes = new Dictionary<CamlQuerySchemaAttributes, object>();
            var query = new CamlQuery();

            if (Field != null)
            {
                // Lookup field actions
                var lookupField = _field as SPFieldLookup;
                if (lookupField != null)
                {
                    if (lookupField.CountRelated)
                        fieldType = SPFieldType.Integer;

                    if (lookupField.IsDependentLookup)
                        fieldType = SPFieldType.Invalid;
                }

                // Calculated field actions
                var calculatedField = _field as SPFieldCalculated;
                if (calculatedField != null)
                    fieldType = calculatedField.OutputType;
            }

            if (IsFieldValidListField && CamlQueryHelper.IsValidFieldForQuery(FieldType, FieldName) && CamlQueryHelper.TryParseValue(fieldType, ref fieldValue))
            {
                // Setting attributres
                if (IsLookupId)
                {
                    aditionalFieldAttributes.Add(CamlQuerySchemaAttributes.LookupId, true);
                    _elementType = CamlQuerySchemaElements.Eq;
                }

                // Setting paresd value and proper field type
                FieldValue = fieldValue;
                FieldType = fieldType;

                // Creating query
                query = new CamlQuery();
                query.Where = GetQueryCondition(aditionalFieldAttributes, aditionalValueAttributes);
                query.ValidFields.Add(FieldName);
            }
            else
                query.InvalidFields.Add(FieldName);

            query.List = _list;
            return query;
        }

        /// <summary>
        /// Gets the query condition.
        /// </summary>
        /// <param name="aditionalfieldAttributes">The aditionalfield attributes.</param>
        /// <param name="aditionalValueAttributes">The aditional value attributes.</param>
        /// <returns>
        /// Returns query condition as string.
        /// </returns>
        protected virtual string GetQueryCondition(Dictionary<CamlQuerySchemaAttributes, object> aditionalfieldAttributes = null, Dictionary<CamlQuerySchemaAttributes, object> aditionalValueAttributes = null)
        {
            if (aditionalfieldAttributes == null)
                aditionalfieldAttributes = new Dictionary<CamlQuerySchemaAttributes, object>();

            if (!aditionalfieldAttributes.ContainsKey(CamlQuerySchemaAttributes.Name))
                aditionalfieldAttributes.Add(CamlQuerySchemaAttributes.Name, FieldName);

            if (aditionalValueAttributes == null)
                aditionalValueAttributes = new Dictionary<CamlQuerySchemaAttributes, object>();

            if (!aditionalValueAttributes.ContainsKey(CamlQuerySchemaAttributes.Type))
                aditionalValueAttributes.Add(CamlQuerySchemaAttributes.Type, FieldType.ToString());

            var conditionElement = EnumHelper.GetStringValue(_elementType, typeof(CamlQuerySchemaElementsAttribute))
                    .Replace(CamQueryVariables.FieldAttributes, CamlQueryHelper.GetAttributes(aditionalfieldAttributes))
                    .Replace(CamQueryVariables.ValueAttribute, CamlQueryHelper.GetAttributes(aditionalValueAttributes))
                    .Replace(CamQueryVariables.Value, FieldValue);

            return conditionElement;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetQuery(_list).Query;
        }

        #endregion
    }
}
