using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core.QueryBuilder.QuerySchemaElements.Base;
using Microsoft.SharePoint;

namespace FPS.Core.QueryBuilder.Interfaces
{
    /// <summary>
    /// IQueryElement interface for all query elements.
    /// </summary>
    public interface IQueryElement
    {
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// Return query.
        /// </returns>
        CamlQuery GetQuery(SPList list = null);
    }
}
