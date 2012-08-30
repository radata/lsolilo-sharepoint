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
    /// Caml query 'Eq' comparison operator.
    /// </summary>
    public class Eq : CamlQueryComparisonOperator, IQueryElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Eq"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        public Eq(string fieldName, SPFieldType fieldType, string fieldValue)
            : base(fieldName, fieldType, fieldValue, CamlQuerySchemaElements.Eq)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Eq"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        public Eq(string fieldName, string fieldValue)
            : base(fieldName, fieldValue, CamlQuerySchemaElements.Eq)
        { }

        #endregion
    }
}
