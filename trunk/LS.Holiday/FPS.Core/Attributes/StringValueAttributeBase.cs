using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.Core.Attributes
{
    /// <summary>
    /// Abstract class for attribute values.
    /// </summary>
    public abstract class StringValueAttributeBase : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringValueAttributeBase(string value)
        {
            StringValue = value;
        }

        #endregion
    }
}
