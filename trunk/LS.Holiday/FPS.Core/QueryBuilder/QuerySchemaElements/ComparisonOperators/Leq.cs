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
    /// Caml query 'Leq' comparison operator.
    /// </summary>
    public class Leq : CamlQueryComparisonOperator, IQueryElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Leq"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        public Leq(string fieldName, SPFieldType fieldType, string fieldValue)
            : base(fieldName, fieldType, fieldValue, CamlQuerySchemaElements.Leq)
        { }

        #endregion
    }
}
