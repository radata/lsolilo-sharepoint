using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core.Attributes;

namespace FPS.Core.QueryBuilder
{
    /// <summary>
    /// CamlQuery Schema Elements Attribute.
    /// </summary>
    public class CamlQuerySchemaElementsAttribute : StringValueAttributeBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CamlQuerySchemaElementsAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public CamlQuerySchemaElementsAttribute(string value) : base(value) { }

        #endregion
    }
}
