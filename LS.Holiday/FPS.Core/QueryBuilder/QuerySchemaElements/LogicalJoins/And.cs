using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core.QueryBuilder.Enums;
using FPS.Core.QueryBuilder.Interfaces;
using FPS.Core.QueryBuilder.QuerySchemaElements.Base;
using Microsoft.SharePoint;

namespace FPS.Core.QueryBuilder.QuerySchemaElements.LogicalJoins
{
    /// <summary>
    /// Caml query 'And' logical join.
    /// </summary>
    public class And : IQueryElement
    {
        #region Fields

        private IQueryElement _firstElement;
        private IQueryElement _secondElement;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="And"/> class.
        /// </summary>
        /// <param name="firstElement">The first element.</param>
        /// <param name="secondElement">The second element.</param>
        internal And(IQueryElement firstElement, IQueryElement secondElement)
        {
            _firstElement = firstElement;
            _secondElement = secondElement;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// Returns query.
        /// </returns>
        public CamlQuery GetQuery(SPList list)
        {
            return CamlQueryHelper.MergeQueryCondition(CamlQuerySchemaElements.And, _firstElement.GetQuery(list), _secondElement.GetQuery(list));
        }

        #endregion
    }
}
