using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core.QueryBuilder.Interfaces;
using FPS.Core.QueryBuilder.QuerySchemaElements.LogicalJoins;
using Microsoft.SharePoint;

namespace FPS.Core.QueryBuilder
{
    /// <summary>
    /// CamlQuery Extensions class.
    /// </summary>
    public static class CamlQueryExtensions
    {
        #region Methods

        /// <summary>
        /// Joins specified queries using 'And' logcal join.
        /// </summary>
        /// <param name="firstQueryElement">The first query element.</param>
        /// <param name="secondQueryElement">The second query element.</param>
        /// <returns>Returns joined IQueryElement.</returns>
        public static IQueryElement And(this IQueryElement firstQueryElement, IQueryElement secondQueryElement)
        {
            return new And(firstQueryElement, secondQueryElement);
        }

        /// <summary>
        /// Joins specified queries using 'Or' logcal join.
        /// </summary>
        /// <param name="firstQueryElement">The first query element.</param>
        /// <param name="secondQueryElement">The second query element.</param>
        /// <returns>Returns joined IQueryElement.</returns>
        public static IQueryElement Or(this IQueryElement firstQueryElement, IQueryElement secondQueryElement)
        {
            return new Or(firstQueryElement, secondQueryElement);
        }

        #endregion
    }
}
