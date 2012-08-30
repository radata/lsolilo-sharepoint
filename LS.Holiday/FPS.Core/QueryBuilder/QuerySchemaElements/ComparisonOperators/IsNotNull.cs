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
    /// Caml query 'IsNotNull' comparison operator.
    /// </summary>
    public class IsNotNull : CamlQueryComparisonOperator, IQueryElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IsNotNull"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public IsNotNull(string fieldName)
            : base(fieldName, SPFieldType.Invalid, null, CamlQuerySchemaElements.IsNotNull)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <returns>
        /// Returns query.
        /// </returns>
        protected override CamlQuery GetQuery()
        {
            var condition = GetQueryCondition();
            var query = new CamlQuery() { Where = condition };
            query.ValidFields.Add(FieldName);

            return query;
        }

        #endregion
    }
}
