using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core.QueryBuilder.Enums;
using FPS.Core.QueryBuilder.Interfaces;
using FPS.Core.QueryBuilder.QuerySchemaElements.Base;
using Microsoft.SharePoint;

namespace FPS.Core.QueryBuilder.QuerySchemaElements.ComparisonOperators
{
    /// <summary>
    /// Caml query 'Contains' comparison operator.
    /// </summary>
    public class Contains : CamlQueryComparisonOperator, IQueryElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Contains"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        public Contains(string fieldName, SPFieldType fieldType, string fieldValue)
            : base(fieldName, fieldType, fieldValue, CamlQuerySchemaElements.Contains)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contains"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        public Contains(string fieldName, string fieldValue)
            : base(fieldName, fieldValue, CamlQuerySchemaElements.Contains)
        {
        }

        #endregion

        #region Methods

        protected override CamlQuery GetQuery()
        {
            // Date time columns throws exception while using Contains
            if (FieldType != SPFieldType.Note && FieldType != SPFieldType.Text && FieldType != SPFieldType.User && FieldType != SPFieldType.Computed)
                _elementType = CamlQuerySchemaElements.Eq;

            return base.GetQuery();
        }

        #endregion
    }
}
